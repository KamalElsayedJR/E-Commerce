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
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {
        private readonly ECommerceDbContext _dbContext;

        public GenericRepository(ECommerceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task CreateAsync(T entity)
        => await _dbContext.Set<T>().AddAsync(entity);
        public void Delete(T entity)
        => _dbContext.Set<T>().Remove(entity);
        public async Task<IReadOnlyList<T>> GetAllAsync()
        => await _dbContext.Set<T>().ToListAsync();
        public async Task<T> GetByIdAsync(string id)
        => await _dbContext.Set<T>().Where(X => X.Id == id).FirstOrDefaultAsync();
        public void Update(T entity)
        => _dbContext.Set<T>().Update(entity);
    }
}
