using E_Commerce.Application.DTOs.Category;
using E_Commerce.Application.DTOs.Response;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;
        public CategoryController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }
        [Authorize(Roles = nameof(UserRoles.Admin))]
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> CreateCategory([FromBody] CreateOrUpdateCategoryDto dto)
        {
            var result = await _categoryServices.CreateCategoryAsync(dto);
            return StatusCode(result.StatusCode, result);
        }
        [Authorize(Roles = nameof(UserRoles.Admin))]
        [HttpPut("{categoryId}")]
        public async Task<ActionResult<BaseResponse>> UpdateCategory([FromBody] CreateOrUpdateCategoryDto dto, [FromRoute] string categoryId)
        {
            var result = await _categoryServices.UpdateCategoryAsync(dto, categoryId);
            return StatusCode(result.StatusCode, result);
        }
        [Authorize(Roles = nameof(UserRoles.Admin))]
        [HttpDelete("{categoryId}")]
        public async Task<ActionResult<BaseResponse>> DeleteCategory([FromRoute] string categoryId)
        {
            var result = await _categoryServices.DeleteCategroyAsync(categoryId);
            return StatusCode(result.StatusCode, result);

        }
        [Authorize]
        [HttpGet("Categories")]
        public async Task<ActionResult<DataResponse<List<CategoryDto>>>> GetAllCategories()
        {
            var result = await _categoryServices.GetAllCategoriesAsync();
            return StatusCode(result.StatusCode, result);
        }
        [Authorize]
        [HttpGet("{categoryId}")]
        public async Task<ActionResult<BaseResponse>> GetCategory([FromRoute] string categoryId)
        {
            var result = await _categoryServices.GetCategoryAsync(categoryId);
            return StatusCode(result.StatusCode, result);
        }
    }
}
