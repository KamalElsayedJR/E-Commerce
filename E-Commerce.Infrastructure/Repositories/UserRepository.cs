using E_Commerce.Domain.Interfaces;
using E_Commerce.Domain.Models;
using E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ECommerceDbContext _dbContext;
        
        public UserRepository(ECommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User?> GetUserByEmailAsync(string Email)
        => await _dbContext.Users.Include(u=>u.RefreshTokens).FirstOrDefaultAsync(u=>u.Email == Email);

        public async Task<bool> IsUserAlreadyExistAsync(string Email)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == Email)) return true;
            return false;
        }
    }
}
