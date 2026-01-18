using E_Commerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseModel
    {
        public Expression<Func<T, bool>> Criteria { get ;}
        public List<Expression<Func<T, object>>> Includes { get ;} = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderByAsce { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get ; set; }
        private int skip;
        public int Skip
        {
            get => skip;
            set => skip = value < 0 ? 0 : value;
        }

        private int take;
        public int Take
        {
            get => take;
            set => take = value <= 0 ? 10 : Math.Min(value, 50);
        }
        public bool IsPaginationAllowed { get ; set ; } = false;

        //all
        public BaseSpecification()
        {
            
        }
        //by criteria
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
        public void ApplyOrderByAsce(Expression<Func<T,object>> orederByAsce)
        {
            OrderByAsce = orederByAsce;
        }
        public void ApplyOrderByDesc(Expression<Func<T,object>> orderByDesc)
        {
            OrderByDesc = orderByDesc;
        }
        public void ApplayPagination(int pageindex,int pagesize)
        {
            IsPaginationAllowed = true;
            Skip = (pageindex - 1) * pagesize;
            Take = pagesize;
        }

    }
}
