using E_Commerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string Email);
        Task<bool> IsUserAlreadyExistAsync(string Email);
    }
}
