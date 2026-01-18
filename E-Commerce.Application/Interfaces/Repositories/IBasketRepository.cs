using E_Commerce.Application.DTOs;
using E_Commerce.Application.DTOs.Basket;
using E_Commerce.Application.DTOs.Response;
using E_Commerce.Domain.Models;
using E_Commerce.Domain.Models.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces.Repositories
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string basketId);
        Task<bool> UpdateBasketAsync(CustomerBasket dto);
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
