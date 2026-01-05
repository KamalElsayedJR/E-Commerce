using E_Commerce.Application.DTOs.Category;
using E_Commerce.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;

        public CategoryController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }
        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateOrUpdateCategoryDto dto)
        {
            var result = await _categoryServices.CreateCategoryAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPut("UpdateCategory/{categoryId}")]
        public async Task<IActionResult> UpdateCategory([FromBody] CreateOrUpdateCategoryDto dto, [FromRoute] string categoryId)
        {
            var result = await _categoryServices.UpdateCategoryAsync(dto, categoryId);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete("DeleteCategory/{categoryId}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] string categoryId)
        {
            var result = await _categoryServices.DeleteCategroyAsync(categoryId);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _categoryServices.GetAllCategoriesAsync();
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet("GetCategory/{categoryId}")]
        public async Task<IActionResult> GetCategory([FromRoute] string categoryId)
        {
            var result = await _categoryServices.GetCategoryAsync(categoryId);
            if (!result.IsSuccess)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
