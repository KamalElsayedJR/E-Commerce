using AutoMapper;
using E_Commerce.Application.DTOs.Auth;
using E_Commerce.Application.DTOs.Basket;
using E_Commerce.Application.DTOs.Category;
using E_Commerce.Application.DTOs.Order;
using E_Commerce.Application.DTOs.Product;
using E_Commerce.Application.DTOs.User;
using E_Commerce.Domain.Models;
using E_Commerce.Domain.Models.Basket;
using E_Commerce.Domain.Models.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Address = E_Commerce.Domain.Models.Address;

namespace E_Commerce.Application.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            // Create your object-object mappings here
            CreateMap<RegisterDto, User>().ForMember(des=>des.HashedPassword ,otc=>otc.MapFrom(src=>src.Password));
            CreateMap<User, UserDto>();
            CreateMap<E_Commerce.Domain.Models.Address, AddressDto>();
            CreateMap<CreateOrUpdateCategoryDto, Category>();
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName));
            CreateMap<Product, Pagination>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName));
            CreateMap<CustomerBasket,CustomerBasketDto>()
                     .ForMember(dest=> dest.Items,opt=>opt.MapFrom(src=>src.Items));
            CreateMap<BasketItem,BasketItemDto>().ReverseMap();
            CreateMap<CreateOrUpdateBasketDto, CustomerBasket>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
            CreateMap<AddressDto, E_Commerce.Domain.Models.OrderAggregate.Address>();
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.DeliveryMethodName, opt => opt.MapFrom(src => src.DeliveryMethod.ShortName))
                .ForMember(dest => dest.ShipToAddress, opt => opt.MapFrom(src => src.ShipToAddress))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.GetTotal()));
            CreateMap<OrderItemDto, OrderItem>();
        }
    }
}
