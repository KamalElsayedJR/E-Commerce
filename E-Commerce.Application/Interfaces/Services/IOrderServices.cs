using E_Commerce.Application.DTOs.Order;
using E_Commerce.Application.DTOs.Response;
using E_Commerce.Domain.Models.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces.Services
{
    public interface IOrderServices
    {
        public Task<BaseResponse> CreateOrderAsync(CreateOrderDto dto, string buyerEmail);
        public Task<DataResponse<List<OrderDto>>> GetOrdersForSpecificUserAsync(string buyerEmail);
        public Task<DataResponse<OrderDto>> GetOrderByIdForSpecificUserAsync(string buyerEmail,string orderId);
        public Task<DataResponse<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethodsAsync();

    }
}
