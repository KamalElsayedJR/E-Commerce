using E_Commerce.Application.DTOs;
using E_Commerce.Application.DTOs.Response;
using E_Commerce.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<DataResponse<UserDto>>> loginAsync(LoginDto dto)
        {
            return await _authServices.LoginAsync(dto);
        }
    }
}
