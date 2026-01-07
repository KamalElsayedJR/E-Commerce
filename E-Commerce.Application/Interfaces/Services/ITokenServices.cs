using E_Commerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces.Services
{
    public interface ITokenServices
    {
        Task<string> GenerateAccessTokenAsync(User user);
        Task<string> GenerateRereshTokenAsync(); 
    }
}
