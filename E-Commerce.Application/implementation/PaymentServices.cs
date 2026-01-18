using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Models.Basket;
using E_Commerce.Domain.Models.OrderAggregate;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.implementation
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _uoW;

        public PaymentServices(IConfiguration configuration,IUnitOfWork UoW)
        {
            _configuration = configuration;
            _uoW = UoW;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            var Basket = await _uoW.BasketRepository.GetBasketAsync(basketId);
            if (Basket == null) return null;
            if(Basket.DeliveryMethodId is null) return null;
            var deliveryMethod = await _uoW.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId);
            var ShippingPrice = deliveryMethod.Cost;
            if(Basket.Items.Count > 0)
            {
                foreach(var item in Basket.Items)
                {
                    var productItem = await _uoW.Repository<Domain.Models.Product>().GetByIdAsync(item.Id);
                    if(item.Price != productItem.Price)
                    {
                        item.Price = productItem.Price;
                    }
                }
            } 
            var Subtotal = Basket.Items.Sum(i => i.Quantity * i.Price);
            var service = new PaymentIntentService();
            PaymentIntent intent;
            if (string.IsNullOrEmpty(Basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)((Subtotal + ShippingPrice) * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                intent = await service.CreateAsync(options);
                Basket.PaymentIntentId = intent.Id;
                Basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)((Subtotal + ShippingPrice) * 100),
                };
                await service.UpdateAsync(Basket.PaymentIntentId, options);
            }
            var result = await _uoW.BasketRepository.UpdateBasketAsync(Basket);
            if (result)
                return Basket;
            return null;

        }

        public async Task<Order?> UpdateOrderPaymentSucceededOrFailed(string paymentIntentId, bool flag)
        {
            var spec = new OrderWithPaymentIntent(paymentIntentId);
            var order = await _uoW.Repository<Order>().GetOneAsyncWithSpec(spec);
            if (order == null) return null;
            if (flag)
            {
                order.Status = OrderStatus.PaymentReceived;
            }
            else
            {
                order.Status = OrderStatus.PaymentFailed;
            }
            _uoW.Repository<Order>().Update(order);
            var result = await _uoW.SaveChangesAsync();
            if (result > 0)
                return order;
            return null;
        }
    }
}
