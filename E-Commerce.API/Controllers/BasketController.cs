using AutoMapper;
using E_Commerce.Application.DTOs.Basket;
using E_Commerce.Application.DTOs.Response;
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Models;
using E_Commerce.Domain.Models.Basket;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepo,IMapper mapper)
        {
            _basketRepo = basketRepo;
            _mapper = mapper;
        }
        [HttpGet("{basketId}")]
        public async Task<ActionResult<DataResponse<CustomerBasketDto>>> GetBasketWithId([FromRoute]string basketId)
        {
            var Basket = await _basketRepo.GetBasketAsync(basketId);
            if (Basket is null) return NotFound(new DataResponse<CustomerBasketDto>(false,404,"Basket Not Found",null));
            var MappedBaskett = _mapper.Map<CustomerBasket,CustomerBasketDto>(Basket);
            return Ok(new DataResponse<CustomerBasketDto>(true,200,"Basket Retrieved Successfully",MappedBaskett));
        }
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> CreateOrUpdateBasket(CreateOrUpdateBasketDto dto)
        {

            var MappedBaket = _mapper.Map<CreateOrUpdateBasketDto, CustomerBasket>(dto);
            
            var Result =  await _basketRepo.UpdateBasketAsync(MappedBaket);
            if (!Result) return BadRequest( new BaseResponse(false,400,"Failed to create or update basket"));
            return Ok( new BaseResponse(true,200,"Basket created or updated successfully"));
        }
        [HttpDelete("{basketId}")]
        public async Task<ActionResult<BaseResponse>> DeleteBasket([FromRoute]string basketId)
        {
            var Result = await _basketRepo.DeleteBasketAsync(basketId);
            if (!Result) return NotFound(new BaseResponse(false,404,"Basket Not Found"));
            return Ok(new BaseResponse(true,200,"Basket Deleted Successfully"));
        }
    }
}
