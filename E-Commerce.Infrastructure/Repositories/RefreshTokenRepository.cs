using E_Commerce.Application.Interfaces.Repositories;
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
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ECommerceDbContext _dbContext;

        public RefreshTokenRepository(ECommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
        =>  await _dbContext.RefreshTokens.Include(rt=>rt.User).FirstOrDefaultAsync(rt => rt.Token == token);
    }
}
