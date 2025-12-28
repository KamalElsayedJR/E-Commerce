using AutoMapper;
using E_Commerce.Application.DTOs;
using E_Commerce.Application.DTOs.Response;
using E_Commerce.Application.Interfaces;
using E_Commerce.Domain.Interfaces;
using E_Commerce.Domain.Models;
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
        public AuthServices(IUnitOfWork UoW,IMapper mapper)
        {
            _uoW = UoW;
            _mapper = mapper;
        }
        public string HashPassword(string Password)
        => BCrypt.Net.BCrypt.HashPassword(Password);
        public async Task<DataResponse<UserDto>> LoginAsync(LoginDto dto)
        {
            if(!await _uoW.UserRepository.IsUserAlreadyExistAsync(dto.Email)) return new DataResponse<UserDto>
                    (false,"UnAuthorized",null,new List<string>()
                                                {"Credintioal Not Correct",
                                                "Email Doesnt Exits",
                                                "Password Not Correct"});
            var loginUser = await _uoW.UserRepository.GetUserByEmailAsync(dto.Email);
            if(!VerifyPassword(dto.Password,loginUser.HashedPassword)) return new DataResponse<UserDto>
                    (false, "UnAuthorized", null, new List<string>()
                                                {"Credintioal Not Correct",
                                                "Email Doesnt Exits",
                                                "Password Not Correct"}); ;
            var MappedUser = _mapper.Map<User, UserDto>(loginUser);
            MappedUser.RefreshToken = "TestRefreshToken";
            MappedUser.AccessToken = "TestAccessToken";
            return new DataResponse<UserDto>(true, "Login does Successfully", new List<UserDto>() { MappedUser }, null); 
        }
        public async Task<BaseResponse> RegisterAsync(RegisterDto dto)
        {
            if (await _uoW.UserRepository.IsUserAlreadyExistAsync(dto.Email)) return new BaseResponse(false,"Email Is Already Exist");
            dto.Password = HashPassword(dto.Password);
            var MappedUser = _mapper.Map<RegisterDto,User>(dto);
            await _uoW.Repository<User>().CreateAsync(MappedUser);
            var Result = await _uoW.SaveChangesAsync();
            if (Result <= 0) return new BaseResponse(false, "Faild To Register User");
            return new BaseResponse(true, "User Registered Successfully");
        }
        public bool VerifyPassword(string Password, string HashedPassword)
        => BCrypt.Net.BCrypt.Verify(Password, HashedPassword);
    }
}
