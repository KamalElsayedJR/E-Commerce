using E_Commerce.Domain.Models.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    public class OrderWithPaymentIntent:BaseSpecification<Order>
    {
        public OrderWithPaymentIntent(string paymentIntentId) : base(o => o.PaymentIntedntId == paymentIntentId)
        {
            //Includes.Add(o => o.DeliveryMethod);
            //Includes.Add(o => o.Items);
        }
    }
}
