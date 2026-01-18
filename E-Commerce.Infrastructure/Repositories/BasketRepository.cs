using E_Commerce.Application.DTOs;
using E_Commerce.Application.DTOs.Basket;
using E_Commerce.Application.DTOs.Response;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Models;
using E_Commerce.Domain.Models.Basket;
using Microsoft.AspNetCore.Connections;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _redis;

        public BasketRepository(IConnectionMultiplexer redis)
        {
            _redis = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        => await _redis.KeyDeleteAsync(basketId);
        

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var basket = await _redis.StringGetAsync(basketId);
            if(basket.IsNull) return null;
            return JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public Task<bool> UpdateBasketAsync(CustomerBasket basket)
        {
            var JsonBasket = JsonSerializer.Serialize(basket);
            return _redis.StringSetAsync(basket.Id, JsonBasket,TimeSpan.FromDays(1));
        }
    }
}
