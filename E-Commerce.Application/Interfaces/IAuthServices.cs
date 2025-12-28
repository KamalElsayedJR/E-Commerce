using E_Commerce.Application.DTOs;
using E_Commerce.Application.DTOs.Response;
using E_Commerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces
{
    public interface IAuthServices
    {
        string HashPassword(string Password);
        Task<BaseResponse> RegisterAsync(RegisterDto dto);
        bool VerifyPassword(string Password, string HashedPassword);
        Task<DataResponse<UserDto>> LoginAsync(LoginDto dto);
    }
}
