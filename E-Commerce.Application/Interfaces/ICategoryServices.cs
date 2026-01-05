using E_Commerce.Application.DTOs.Category;
using E_Commerce.Application.DTOs.Response;
using E_Commerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces
{
    public interface ICategoryServices
    {
        Task<BaseResponse> CreateCategoryAsync(CreateOrUpdateCategoryDto dto);
        Task<BaseResponse> UpdateCategoryAsync(CreateOrUpdateCategoryDto dto, string CategoryId);
        Task<BaseResponse> DeleteCategroyAsync(string CategoryId);
        Task<DataResponse<List<CategoryDto>>> GetAllCategoriesAsync();
        Task<DataResponse<CategoryDto>> GetCategoryAsync(string CategoryId);
    }
}
