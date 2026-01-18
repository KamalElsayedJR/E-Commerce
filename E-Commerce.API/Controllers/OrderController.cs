using E_Commerce.Application.DTOs.Order;
using E_Commerce.Application.DTOs.Response;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Models.OrderAggregate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;

        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> CreateOrder([FromBody] CreateOrderDto dto, string buyerEmail)
        {
            buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var resposne = await _orderServices.CreateOrderAsync(dto, buyerEmail);
            return StatusCode(resposne.StatusCode, resposne);
        }
        [HttpGet]
        public async Task<ActionResult<BaseResponse>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var response = await _orderServices.GetOrdersForSpecificUserAsync(buyerEmail);
            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse>> GetOrderByIdForUser(string userId)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var response = await _orderServices.GetOrderByIdForSpecificUserAsync(userId, buyerEmail);
            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("delivery-method")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var response = await _orderServices.GetDeliveryMethodsAsync();
            return StatusCode(response.StatusCode, response);
        }
    }
}
