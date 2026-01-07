using E_Commerce.Application.DTOs.Product;
using E_Commerce.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Interfaces.Services
{
    public interface ISelllerProductServices
    {
        Task<BaseResponse> CreateNewProduct(CreateProductDto dto,string userId);
        Task<BaseResponse> UpdateProduct(UpdateProductDto dto, string productId,string userId);
    }
}
