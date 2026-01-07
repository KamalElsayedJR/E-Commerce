using E_Commerce.Application.DTOs.Product;
using E_Commerce.Application.DTOs.Response;
using E_Commerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ISelllerProductServices _selllerProductServices;

        public ProductController(ISelllerProductServices selllerProductServices)
        {
            _selllerProductServices = selllerProductServices;
        }
        [Authorize(Roles = "Seller")]
        [HttpPost("AddNewProduct")]
        public async Task<ActionResult<BaseResponse>> AddProduct(CreateProductDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized(new BaseResponse(false, "User not authorized"));
            var response = await _selllerProductServices.CreateNewProduct(dto, userId);
            if (!response.IsSuccess) return BadRequest(response);
            return Ok(response);
        }
    }
}
