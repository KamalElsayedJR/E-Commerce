using E_Commerce.Domain.Models.Basket;
using E_Commerce.Domain.Models.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces.Services
{
    public interface IPaymentServices
    {
        Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId);
        Task<Order?> UpdateOrderPaymentSucceededOrFailed(string paymentIntentId,bool flag);
    }
}
