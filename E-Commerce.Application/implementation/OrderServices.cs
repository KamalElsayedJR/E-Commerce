using AutoMapper;
using E_Commerce.Application.DTOs.Order;
using E_Commerce.Application.DTOs.Response;
using E_Commerce.Application.DTOs.User;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Models;
using E_Commerce.Domain.Models.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.implementation
{
    public class OrderServices : IOrderServices
    {
        private readonly IUnitOfWork _uoW;
        private readonly IMapper _mapper;

        public OrderServices(IUnitOfWork UoW,IMapper mapper)
        {
            _uoW = UoW;
            _mapper = mapper;
        }
        public async Task<BaseResponse> CreateOrderAsync(CreateOrderDto dto, string buyerEmail)
        {
            var Basket = await _uoW.BasketRepository.GetBasketAsync(dto.BasketId);
            if (Basket is null || Basket.Items.Count <= 0)
            {
                return new BaseResponse(false, 400, "Basket is empty");
            }

            var orderItems = new List<OrderItem>();
            foreach (var item in Basket.Items)
            {
                var product = await _uoW.Repository<Product>().GetByIdAsync(item.Id);
                if (product is not null && product.Stock >= item.Quantity)
                {
                    orderItems.Add(new OrderItem()
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        ImageUrl = product.ImagesUrl,
                        Price = product.Price,
                        Quantity = item.Quantity
                    });
                }
            }
            var deliveryMethod = await _uoW.Repository<DeliveryMethod>().GetByIdAsync(dto.DeliveryMethodId);
            if (deliveryMethod is null)
            {
                return new BaseResponse(false, 400, "Invalid delivery method");
            }
            var MappedAddress = _mapper.Map<AddressDto, E_Commerce.Domain.Models.OrderAggregate.Address>(dto.ShippingAddress);
            var order = new Order()
            {
                BuyerEmail = buyerEmail,
                DeliveryMethodId = dto.DeliveryMethodId,
                ShipToAddress = MappedAddress,
                Items = orderItems,
                DeliveryMethod = deliveryMethod,
                Subtotal = orderItems.Sum(item => item.Price * item.Quantity)
            };
            await _uoW.Repository<Order>().CreateAsync(order);
            var result = await _uoW.SaveChangesAsync();
            if (result <= 0) return new BaseResponse(false, 500, "Failed to create order");
            return new BaseResponse(true, 200, "Order created successfully");
        }
        public async Task<DataResponse<OrderDto>> GetOrderByIdForSpecificUserAsync(string buyerEmail, string orderId)
        {
            var spec = new OrderSpecification(buyerEmail, orderId);
            var order = await _uoW.Repository<Order>().GetOneAsyncWithSpec(spec);
            if (order is null) return new DataResponse<OrderDto>(false, 404, "Order not found",null);
            var MappedOrder = _mapper.Map<Order, OrderDto>(order);
            return new DataResponse<OrderDto>(true, 200, "Order retrieved successfully", MappedOrder);
        }
        public async Task<DataResponse<List<OrderDto>>> GetOrdersForSpecificUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecification(buyerEmail);
            var Orders = await _uoW.Repository<Order>().GetAllAsyncWithSpec(spec);
            var MappedOrders = _mapper.Map<IReadOnlyList<Order>, List<OrderDto>>(Orders);
            return new DataResponse<List<OrderDto>>(true, 200, "Orders retrieved successfully", MappedOrders);
        }
        public async Task<DataResponse<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await _uoW.Repository<DeliveryMethod>().GetAllAsync();
            return new DataResponse<IReadOnlyList<DeliveryMethod>>(true, 200, "Delivery methods retrieved successfully", deliveryMethods);
        }
    }
}
