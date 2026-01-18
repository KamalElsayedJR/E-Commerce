using E_Commerce.Application.DTOs;
using E_Commerce.Application.DTOs.Auth;
using E_Commerce.Application.DTOs.Response;
using E_Commerce.Application.DTOs.User;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;

        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }
        [HttpPost("register")]
        public async Task<ActionResult<BaseResponse>> RegisterAsync(RegisterDto dto)
        {
            var response = await _authServices.RegisterAsync(dto);
            if(!response.IsSuccess) return BadRequest(response);
            return Ok(response);
        }
        [HttpPost("login")]
        public async Task<ActionResult<DataResponse<AuthDtoResponse>>> loginAsync(LoginDto dto)
        {
            var response = await _authServices.LoginAsync(dto);
            if(!response.IsSuccess) return Unauthorized(response);
            var cookiesOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = response.Data.RefreshTokenExpierAt,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };
            Response.Cookies.Append("refreshToken", response.Data.RefreshToken, cookiesOptions);
            return Ok(response);
        }
        [HttpPost("refreshtoken")]
        public async Task<ActionResult<TokenReponse>> RefreshTokenAsync()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized(new DataResponse<TokenReponse>
                    (false, 401,"No Refresh Token Provided", null, 
                    new List<string>() { "No Refresh Token Provided" }));
            }
            var response = await _authServices.RefreshTokenAsync(refreshToken);
            if(!response.IsSuccess) return Unauthorized(response);
            var cookiesOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
                Secure = true,
                SameSite = SameSiteMode.Strict
            };
            Response.Cookies.Append("refreshToken", response.Data.RefreshToken, cookiesOptions);
            return Ok(response);
        }
        [HttpPost("logout")]
        public async Task<ActionResult<BaseResponse>> LogOutAsync()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await _authServices.LogOutAsync(refreshToken);
            if(!response.IsSuccess) return BadRequest(response);
            Response.Cookies.Delete("refreshToken");
            return Ok(response);
        }
        [Authorize]
        [HttpGet("Profile")]
        public async Task<ActionResult<DataResponse<UserDto>>> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new DataResponse<UserDto>
                    (false, 401,"Invalid User", null, 
                    new List<string>() { "Invalid User" }));
            }
            var response = await _authServices.Me(userId);
            if(!response.IsSuccess) return Unauthorized(response);
            return Ok(response);
        }
        [Authorize]
        [HttpPut("Address/{addressId}")]
        public async Task<ActionResult<BaseResponse>> UpdateAddress(AddressDto dto,[FromRoute]string addressId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _authServices.UpdateAddress(dto,addressId, userId);
            return StatusCode(response.StatusCode, response);
        }

    }
}
