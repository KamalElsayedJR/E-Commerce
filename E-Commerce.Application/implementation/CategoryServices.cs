using AutoMapper;
using E_Commerce.Application.DTOs.Category;
using E_Commerce.Application.DTOs.Response;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.implementation
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IUnitOfWork _uoW;
        private readonly IMapper _mapper;

        public CategoryServices(IUnitOfWork UoW,IMapper mapper)
        {
            _uoW = UoW;
            _mapper = mapper;
        }
        public async Task<DataResponse<List<CategoryDto>>> GetAllCategoriesAsync()
        {
            var cats = (await _uoW.Repository<Category>().GetAllAsync()).Where(c=>c.IsActive);
            if (cats is null) return new DataResponse<List<CategoryDto>>(false, 200,"No Categroy Found",null);
            var MappedCats = _mapper.Map<IEnumerable<Category>, List<CategoryDto>>(cats);
            return new DataResponse<List<CategoryDto>>(true, 200,"Categories Retrieved Successfully",
                                                        MappedCats,
                                                        null);
        }
        public async Task<BaseResponse> CreateCategoryAsync(CreateOrUpdateCategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) return new BaseResponse(false,400,"Invalid Form");
            var cat = _mapper.Map<CreateOrUpdateCategoryDto, Category>(dto);
            await _uoW.Repository<Category>().CreateAsync(cat);
            var result = await _uoW.SaveChangesAsync();
            if (result <= 0) return new BaseResponse(false,500,$"Cant Add {dto.Name}",new List<string>() {"Cant Save This Category"});
            return new BaseResponse(true,200,$"{dto.Name} Created Successfully");
        }
        public async Task<BaseResponse> DeleteCategroyAsync(string CategroyId)
        {
            var cat = await _uoW.Repository<Category>().GetByIdAsync(CategroyId);
            if (cat == null) return new BaseResponse(false, 404,"Category Not Found", null);
            _uoW.Repository<Category>().Delete(cat);
            var result =  await _uoW.SaveChangesAsync();
            if (result <= 0) return new BaseResponse(false, 500,$"Cant Delete {cat.Name}", null);
            return (new BaseResponse(true, 200,$"{cat.Name} Deleted Successfully"));
        }
        public async Task<BaseResponse> UpdateCategoryAsync(CreateOrUpdateCategoryDto dto, string CategoryId)
        {
            var cat = await _uoW.Repository<Category>().GetByIdAsync(CategoryId);
            if (cat == null) return new BaseResponse(false, 404,"Category Not Found", null);
            _mapper.Map(dto, cat);
            _uoW.Repository<Category>().Update(cat);
            var result = await _uoW.SaveChangesAsync();
            if (result <= 0) return new BaseResponse(false, 500,$"Cant Update {dto.Name}", null);
            return new BaseResponse(true,200,$"{dto.Name} Updated Successfully", null);
        }
        public async Task<DataResponse<CategoryDto>> GetCategoryAsync(string CategoryId)
        {
            var cat = await _uoW.Repository<Category>().GetByIdAsync(CategoryId);
            if (cat is null) return new DataResponse<CategoryDto>(false,404,"Category Not Found",
                                                                 null,
                                                                 new List<string>() {$"No Category With {CategoryId} Id"});
            var MappedCat = _mapper.Map<Category,CategoryDto>(cat);
            return new DataResponse<CategoryDto>(true,200,"Category Retrieved Successfully",
                                                 MappedCat,
                                                 null);
        }
    }
}
