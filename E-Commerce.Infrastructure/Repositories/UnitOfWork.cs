using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Models;
using E_Commerce.Infrastructure.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserRepository UserRepository { get ; }

        public IRefreshTokenRepository RefreshTokenRepository { get; }

        private readonly ECommerceDbContext _dbContext;
        private readonly Hashtable _repo = new Hashtable();

        public UnitOfWork(ECommerceDbContext dbContext, IUserRepository userRepo,IRefreshTokenRepository refreshTokenRepository)
        {
            _dbContext = dbContext;
            UserRepository = userRepo;
            RefreshTokenRepository = refreshTokenRepository;
        }
        public async ValueTask DisposeAsync()
        => await _dbContext.DisposeAsync();
        public IGenericRepository<T> Repository<T>() where T : BaseModel
        {
            var type = typeof(T).Name;
            if (!_repo.ContainsKey(type))
            {
                var repo = new GenericRepository<T>(_dbContext);
                _repo.Add(type, repo);
            }
            return (IGenericRepository<T>)_repo[type];
        }
        public async Task<int> SaveChangesAsync()
        => await _dbContext.SaveChangesAsync();
    }
}
