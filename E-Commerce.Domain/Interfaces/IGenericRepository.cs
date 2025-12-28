using E_Commerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : BaseModel
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
        Task CreateAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
