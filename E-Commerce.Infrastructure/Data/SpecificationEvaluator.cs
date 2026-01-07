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
            if (spec.Criteria != null) query = query.Where(spec.Criteria);
            query = spec.Includes.Aggregate(query,(current,includes)=>current.Include(includes));
            return query;
        }

    }
}
