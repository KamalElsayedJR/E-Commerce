using E_Commerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseModel;
        public IUserRepository UserRepository { get;}
        public IRefreshTokenRepository RefreshTokenRepository { get;}
        public IBasketRepository BasketRepository { get;  }
        Task<int> SaveChangesAsync();
    }
}
