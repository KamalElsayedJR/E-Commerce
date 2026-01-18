using E_Commerce.Application.DTOs.Product;
using E_Commerce.Application.DTOs.Response;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces.Services
{
    public interface IProductServices
    {
        Task<BaseResponse> CreateNewProductAsync(CreateProductDto dto,string userId);
        Task<BaseResponse> UpdateProductAsync(UpdateProductDto dto, string productId,string userId);
        Task<BaseResponse> DeletePrdouctAsync(string productId, string userId);
        Task<DataResponse<List<Pagination>>> GetProducts(string? sort,string? search, int pageIndex, int pageSize);
        Task<DataResponse<ProductDto>> GetProduct(string productId);
        Task<BaseResponse> UpdateProductStatus(string productId,bool status);
    }
}
