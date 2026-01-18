using AutoMapper;
using E_Commerce.Application.DTOs.Auth;
using E_Commerce.Application.DTOs.Response;
using E_Commerce.Application.DTOs.User;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Models;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.implementation
{
    public class AuthServices : IAuthServices
    {
        private readonly IUnitOfWork _uoW;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;
        public AuthServices(IUnitOfWork UoW,IMapper mapper,ITokenServices tokenServices)
        {
            _uoW = UoW;
            _mapper = mapper;
            _tokenServices = tokenServices;
        }
        public string HashPassword(string Password)
        => BCrypt.Net.BCrypt.HashPassword(Password);
        public async Task<DataResponse<AuthDtoResponse>> LoginAsync(LoginDto dto)
        {
            var loginUser = await _uoW.UserRepository.GetUserByEmailAsync(dto.Email);
            if(loginUser is null || !VerifyPassword(dto.Password,loginUser.HashedPassword)) return new DataResponse<AuthDtoResponse>
                    (false, 401,"You Can't login", null, new List<string>()
                                                {"Invalid email or password"}); ;
            var MappedUser = _mapper.Map<User, UserDto>(loginUser);
            var result = new AuthDtoResponse()
            {
                AccessToken =  await _tokenServices.GenerateAccessTokenAsync(loginUser),
                User = MappedUser
            };
                #region RefreshToken
            var activeRefreshToken= loginUser.RefreshTokens.FirstOrDefault(rt=>rt.IsActive);
            if (activeRefreshToken is null)
                {
                    var RefreshToken = await _tokenServices.GenerateRereshTokenAsync();
                    var TokenEntity = new RefreshToken
                    {
                        Token = RefreshToken,
                        ExpiresAt = DateTime.UtcNow.AddDays(7),
                        UserId = loginUser.Id
                    };
                    await _uoW.Repository<RefreshToken>().CreateAsync(TokenEntity);
                    if (await _uoW.SaveChangesAsync() <= 0)
                    {
                        return new DataResponse<AuthDtoResponse>(false, 500,"Cant Login Right Now", null, new List<string>() { "Cant Save Refresh Token" });
                    }
                result.RefreshToken = RefreshToken;
                result.RefreshTokenExpierAt = TokenEntity.ExpiresAt;
                }
                result.RefreshToken = loginUser.RefreshTokens.FirstOrDefault(rt=>rt.IsActive).Token;
                result.RefreshTokenExpierAt = loginUser.RefreshTokens.FirstOrDefault(rt => rt.IsActive).ExpiresAt;
                #endregion
            return new DataResponse<AuthDtoResponse>(true,200,"Login does Successfully", result); 
        }
        public async Task<BaseResponse> LogOutAsync(string token)
        {
            var tokenEntity = await _uoW.RefreshTokenRepository.GetRefreshTokenAsync(token);
            if(tokenEntity is null) return new BaseResponse(false,401,"Invalid Token");
            if (tokenEntity.IsActive)
            {
                tokenEntity.RevokedAt = DateTime.UtcNow;
                _uoW.Repository<RefreshToken>().Delete(tokenEntity);
                var result = await _uoW.SaveChangesAsync();
                if (result <= 0) return new BaseResponse(false, 401,"Faild To Logout", new List<string>() { "Cant delete Refresh Token" });
            }
            return new BaseResponse(true, 200,"Logged Out Successfully", null);
        }
        public async Task<DataResponse<UserDto>> Me(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new DataResponse<UserDto>(false, 401,"UnAuthorized", null, new List<string>() { "User Not Found" });
            }
            var spec = new UserSpecification(userId);
            var  user = await _uoW.Repository<User>().GetOneAsyncWithSpec(spec);
            //var user = await _uoW.Repository<User>().GetByIdAsync(userId);
            if (user is null)
            {
                return new DataResponse<UserDto>(false, 401, "UnAuthorized", null, new List<string>() { "User Not Found" });
            }
            var mappedUser = _mapper.Map<User, UserDto>(user);
            return new DataResponse<UserDto>(true, 200, "User Found Successfully", mappedUser);
        }
        public async Task<DataResponse<TokenReponse>> RefreshTokenAsync(string token)
        {
            var refreshToken = (await _uoW.RefreshTokenRepository.GetRefreshTokenAsync(token));
            if (refreshToken is null)
            {
                return new DataResponse<TokenReponse>(false, 400, null,
                                                      null, 
                                                      new List<string>() { "Invalid Refresh Token" });
            }
            if (refreshToken.IsActive)
            {
                refreshToken.RevokedAt = DateTime.UtcNow;
                _uoW.Repository<RefreshToken>().Delete(refreshToken);
                var newRefreshToken = await _tokenServices.GenerateRereshTokenAsync();
                var newTokenEntity = new RefreshToken
                {
                    Token = newRefreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    UserId = refreshToken.UserId,
                    User = refreshToken.User
                };
                await _uoW.Repository<RefreshToken>().CreateAsync(newTokenEntity);
                var Result = await _uoW.SaveChangesAsync();
                if (Result <= 0) return new DataResponse<TokenReponse>(false, 500,"Invalid Refresh Token", 
                                                                      null, 
                                                                      new List<string>() { "Cant Save/Refresh Refresh Token" });
                var accessToken = await _tokenServices.GenerateAccessTokenAsync(refreshToken.User);
                var tokens = new TokenReponse
                {
                    AccessToken = accessToken,
                    RefreshToken = newRefreshToken
                };
                return new DataResponse<TokenReponse>(isSuccess: true,statusCode:200,
                                                     message: "Refreshtoken Refreshed Successfully",
                                                     tokens, null);
            }
            return new DataResponse<TokenReponse>(false,401 ,"Invalid Refresh Token", 
                                                  null, 
                                                  new List<string>() { "Invalid Refresh Token" });
        }
        public async Task<BaseResponse> RegisterAsync(RegisterDto dto)
        {
            if (await _uoW.UserRepository.IsUserAlreadyExistAsync(dto.Email)) return new BaseResponse(false,400,"Email Is Already Exist");
            dto.Password = HashPassword(dto.Password);
            var MappedUser = _mapper.Map<RegisterDto,User>(dto);
            await _uoW.Repository<User>().CreateAsync(MappedUser);
            var Result = await _uoW.SaveChangesAsync();
            if (Result <= 0) return new BaseResponse(false,500 ,"Faild To Register User", new List<string>() { "Cant Save This User" });
            return new BaseResponse(true, 200,"User Registered Successfully");
        }
        public async Task<BaseResponse> UpdateAddress(AddressDto dto,string addressId,string userId)
        {
            var addressEntity =await _uoW.Repository<Address>().GetByIdAsync(addressId);
            if(addressEntity is null || addressEntity.UserId != userId)
                return new BaseResponse(false, 404, "Address Not Found");
            var address = _mapper.Map<AddressDto, Address>(dto);
            _uoW.Repository<Address>().Update(address);
            var result = await _uoW.SaveChangesAsync();
            if (result <= 0) return new BaseResponse(false, 500, "Faild To Update Address", new List<string>() { "Cant Update Address" });
            return new BaseResponse(true, 200, "Address Updated Successfully");
        }

        public bool VerifyPassword(string Password, string HashedPassword)
        => BCrypt.Net.BCrypt.Verify(Password, HashedPassword);
    }
}
