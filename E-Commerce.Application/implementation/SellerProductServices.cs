using AutoMapper;
using E_Commerce.Application.DTOs.Product;
using E_Commerce.Application.DTOs.Response;
using E_Commerce.Application.Interfaces.Image;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.implementation
{
    public class SellerProductServices : ISelllerProductServices
    {
        private readonly IUnitOfWork _uoW;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public SellerProductServices(IUnitOfWork UoW,IMapper mapper,IImageService imageService)
        {
            _uoW = UoW;
            _mapper = mapper;
            _imageService = imageService;
        }
        public async Task<BaseResponse> CreateNewProduct(CreateProductDto dto,string userId)
        {
            if (dto is null) return new BaseResponse(false,"Cant Send Empty Request");
            if (dto.Images is null || !dto.Images.Any()) return new BaseResponse(false,"Images Is Requierd");
            var cat = await _uoW.Repository<Category>().GetByIdAsync(dto.CategoryId);
            if (cat is null) return new BaseResponse(false,"Invalid Category");
            var validationResult = _imageService.Validate(dto.Images);
            if (!validationResult.IsSuccess) return validationResult;
            var imageUrl = await _imageService.UploadImagesAsync(dto.Images);
            var ProductEntity = _mapper.Map<CreateProductDto,Product>(dto);
            ProductEntity.UserId = userId;
            ProductEntity.Status = ProductStatus.Pending;
            ProductEntity.ImagesUrl = imageUrl;
            await _uoW.Repository<Product>().CreateAsync(ProductEntity);
            if (await _uoW.SaveChangesAsync() <= 0) return new BaseResponse(false, "Cant Add new Product");
            return new BaseResponse(true,"Product Added Successfully");
        }

        public async Task<BaseResponse> UpdateProduct(UpdateProductDto dto, string productId,string userId)
        {
            var product = await _uoW.Repository<Product>().GetByIdAsync(productId);
            if (product is null) return new BaseResponse(false,"Product Not Found");
            
            throw new NotImplementedException();
        }
    }
}
