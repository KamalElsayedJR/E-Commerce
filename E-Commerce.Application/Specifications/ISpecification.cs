using E_Commerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    public interface ISpecification<T> where T : BaseModel
    {
        public Expression<Func<T,bool>> Criteria { get;}
        public List<Expression<Func<T,object>>> Includes { get;}
        public Expression<Func<T,object>> OrderByAsce { get; set; }
        public Expression<Func<T,object>> OrderByDesc { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationAllowed { get; set; }

    }
}
