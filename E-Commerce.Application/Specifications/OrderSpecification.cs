using E_Commerce.Domain.Models.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    public class OrderSpecification : BaseSpecification<Order>
    {
        public OrderSpecification(string Email,string userId):base(o=>o.BuyerEmail == Email && o.Id == userId)
        {
            Includes.Add(o=>o.DeliveryMethod);
            Includes.Add(o=>o.Items);
            Includes.Add(o=>o.ShipToAddress);
        }
        public OrderSpecification(string Email):base(o=>o.BuyerEmail == Email)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
            Includes.Add(o => o.ShipToAddress);
            ApplyOrderByDesc(o => o.CreatedAt);
        }


    }
}
