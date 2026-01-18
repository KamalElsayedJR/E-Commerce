using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Data
{
    public static class SpecificationEvaluator<T> where T : BaseModel
    {
        public static IQueryable<T> GetQuery(IQueryable<T> InptQuery,ISpecification<T> spec)
        {
            var query = InptQuery;
            if (spec.Criteria is not null) query = query.Where(spec.Criteria);
            if (spec.OrderByDesc is not null) query = query.OrderByDescending(spec.OrderByDesc);
            if (spec.OrderByAsce is not null) query = query.OrderBy(spec.OrderByAsce);
            if (spec.IsPaginationAllowed) query = query.Skip(spec.Skip).Take(spec.Take);
            query = spec.Includes.Aggregate(query,(current,includes)=>current.Include(includes));
            return query;
        }
    }
}
