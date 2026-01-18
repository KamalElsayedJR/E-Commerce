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
        private readonly IProductServices _ProductServices;
        public ProductController(IProductServices ProductServices)
        {
            _ProductServices = ProductServices;
        }
        [Authorize(Roles = "Seller")]
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> AddProduct(CreateProductDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized(new BaseResponse(false, 401, "User not authorized"));
            var response = await _ProductServices.CreateNewProductAsync(dto, userId);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = "Seller")]
        [HttpPut("{productId}")]
        public async Task<ActionResult<BaseResponse>> UpdateProduct(UpdateProductDto dto, [FromRoute] string productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return Unauthorized(new BaseResponse(false, 401, "User not authorized", null));
            var response = await _ProductServices.UpdateProductAsync(dto, productId, userId);
            return StatusCode(response.StatusCode, response);
        }
        [HttpDelete("{productId}")]
        [Authorize(Roles = "Seller")]
        public async Task<ActionResult<BaseResponse>> DeleteProduct([FromRoute] string productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return Unauthorized(new BaseResponse(false, 401, "User not authorized", null));
            var response = await _ProductServices.DeletePrdouctAsync(productId, userId);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize]
        [HttpGet("{productId}")]
        public async Task<ActionResult<BaseResponse>> GetProductById([FromRoute] string productId)
        {
            var response = await _ProductServices.GetProduct(productId);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<DataResponse<List<ProductDto>>>> GetAllProducts([FromQuery]string? sort,string? search,int pageIndex,int pageSize)
        {
            var response = await _ProductServices.GetProducts(sort, search,pageIndex,pageSize);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("Status/{productId}")]
        public async Task<ActionResult<BaseResponse>> UpdateProductStatus([FromRoute] string productId, [FromQuery] bool status)
        {
            var response = await _ProductServices.UpdateProductStatus(productId, status);
            return StatusCode(response.StatusCode, response);
        }

    }
}
