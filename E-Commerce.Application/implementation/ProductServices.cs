using AutoMapper;
using E_Commerce.Application.DTOs.Product;
using E_Commerce.Application.DTOs.Response;
using E_Commerce.Application.Interfaces.Image;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.implementation
{
    public class ProductServices : IProductServices
    {
        private readonly IUnitOfWork _uoW;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        public ProductServices(IUnitOfWork UoW,IMapper mapper,IImageService imageService)
        {
            _uoW = UoW;
            _mapper = mapper;
            _imageService = imageService;
        }
        public async Task<BaseResponse> CreateNewProductAsync(CreateProductDto dto,string userId)
        {
            if (dto is null) return new BaseResponse(false,400,"Cant Send Empty Request");
            if (dto.Images is null || !dto.Images.Any()) return new BaseResponse(false,400,"Images Is Requierd");
            var cat = await _uoW.Repository<Category>().GetByIdAsync(dto.CategoryId);
            if (cat is null) return new BaseResponse(false,400,"Invalid Category",new List<string>() {"Category Not Found"});
            var validationResult = _imageService.Validate(dto.Images);
            if (!validationResult.IsSuccess) return validationResult;
            var imageUrl = await _imageService.UploadImagesAsync(dto.Images);
            var ProductEntity = _mapper.Map<CreateProductDto,Product>(dto);
            ProductEntity.UserId = userId;
            ProductEntity.Status = ProductStatus.Pending;
            ProductEntity.ImagesUrl = imageUrl;
            await _uoW.Repository<Product>().CreateAsync(ProductEntity);
            if (await _uoW.SaveChangesAsync() <= 0) return new BaseResponse(false, 400, "Cant Add new Product", new List<string>() { "Cant Save Product" });
            return new BaseResponse(true,400, "Product Added Successfully");
        }
        public async Task<BaseResponse> DeletePrdouctAsync(string productId, string userId)
        {
            var productEntity = await _uoW.Repository<Product>().GetByIdAsync(productId);
            if (productEntity is null) return new BaseResponse(false,404);
            if (productEntity.UserId != userId) return new BaseResponse(false,403);
            _uoW.Repository<Product>().Delete(productEntity);
            if (await _uoW.SaveChangesAsync() <= 0) return new BaseResponse(false, 400, "Cant Delete Product", new List<string>() { "Cant Save Changes","0 Rows Affected" });
            foreach (var image in productEntity.ImagesUrl)
            {
                var imageDeletionResponse = await _imageService.DeleteImageAsync(image);
                if (!imageDeletionResponse.IsSuccess)
                {
                    // Log the failure but do not fail the entire operation
                    return new BaseResponse(true, 200, "Product Deleted Successfully, but some images could not be deleted.", new List<string> { imageDeletionResponse.Message });
                }
            }
            return new BaseResponse(true,200,"Product Deleted Successfully");
        }
        public async Task<BaseResponse> UpdateProductAsync(UpdateProductDto dto, string productId,string userId)
        {
            var product = await _uoW.Repository<Product>().GetByIdAsync(productId);
            if (product is null) return new BaseResponse(false, 400, "Product Not Found");
            if (product.UserId != userId) return new BaseResponse(false,401,"UnAuthorized", new List<string>() { "You Are Not Authorized To Be Here" });
            var imagesurl = await _imageService.UploadImagesAsync(dto.Images);
            product.ImagesUrl.AddRange(imagesurl);
            _mapper.Map(dto, product);
            _uoW.Repository<Product>().Update(product);
            if (await _uoW.SaveChangesAsync() <= 0) return new BaseResponse(false, 401,"Cant Update Product", new List<string>() { "Cant Save This Update" });
            return new BaseResponse(true,200,"Product Updated Successfully");
        }
        public async Task<DataResponse<List<Pagination>>> GetProducts(string? sort,string? search,int pageIndex,int pageSize)
        {
            var spec = new ProductSpecification(sort, search,pageIndex,pageSize);
            var products = await _uoW.Repository<Product>().GetAllAsyncWithSpec(spec);
            products.Where(p=>p.Status == ProductStatus.Aproved);
            var dto = new Pagination()
            {
                PageNumber = pageIndex,
                PageSize = pageSize,
                TotalCount = products.Count()
            };
            if (products is null) return new DataResponse<List<Pagination>>(false, 404,"No Products Found",null,new List<string>() { "Possible Errors\n","No Added Products\n","You Are NOt Authorized"});
            var MappedProducts = _mapper.Map<IReadOnlyList<Product>, List<Pagination>>(products);
            return new DataResponse<List<Pagination>>(true, 200, "Products RetrivedSuccessfully",MappedProducts,null);
        }
        public async Task<DataResponse<ProductDto>> GetProduct(string productId)
        {
            var spec = new ProductSpecification(productId);
            var product = await _uoW.Repository<Product>().GetOneAsyncWithSpec(spec);
            if (product is null) return new DataResponse<ProductDto>(false,404,"Product Not Found",null);
            var mappedPro = _mapper.Map<Product, ProductDto>(product);
            return new DataResponse<ProductDto>(true,200,"Product Retrived Successfully", mappedPro);
        }
        public async Task<BaseResponse> UpdateProductStatus(string productId, bool status)
        {
            var product  =await  _uoW.Repository<Product>().GetByIdAsync(productId);
            if (product is null) return new BaseResponse(false,404,"Product Not Found");
            if (status) product.Status = ProductStatus.Aproved;
            product.Status = ProductStatus.Rejected;
            _uoW.Repository<Product>().Update(product);
            var result = await _uoW.SaveChangesAsync();
            if (result <= 0) return new BaseResponse(false,500,"Cant Save Changes",null);
            return new BaseResponse(true,200,"Product Status Updated Successfully");
        }
    }
}
