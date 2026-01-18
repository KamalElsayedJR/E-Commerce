using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        //all
        public ProductSpecification(string? Sort,string? Search,int pageIndex,int pageSize)
            :base(P => (string.IsNullOrEmpty(Search)
            || P.Name.ToLower().Contains(Search.ToLower())) 
            & P.Status == ProductStatus.Aproved
            & P.Stock > 0)
        {
            Includes.Add(p => p.Category);
            Includes.Add(p => p.User);
            if (!string.IsNullOrEmpty(Sort))
            {
                switch (Sort.ToLower())
                {
                    case "priceasce":
                        ApplyOrderByAsce(P => P.Price);
                        break;
                    case "pricedecs":
                        ApplyOrderByDesc(P => P.Price);
                        break;
                    default:
                        ApplyOrderByAsce(P => P.CreatedAt);
                        break;
                }
            }
            ApplayPagination(pageIndex,pageSize);
        }
        //one by id
        public ProductSpecification(string productId) : base(p=>p.Id==productId&p.Status==ProductStatus.Aproved&p.Stock > 0)
        {
            Includes.Add(p => p.Category);
            Includes.Add(p => p.User);
        }

    }
}
