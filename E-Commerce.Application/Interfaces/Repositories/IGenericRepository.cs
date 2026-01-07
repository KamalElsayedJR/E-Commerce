using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : BaseModel
    {

        Task<IReadOnlyList<T>> GetAllAsyncWithSpec(ISpecification<T> spec);
        Task<T> GetOneAsyncWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id); 
        Task CreateAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
