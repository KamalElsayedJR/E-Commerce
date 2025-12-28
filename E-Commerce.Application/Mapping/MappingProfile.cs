using AutoMapper;
using E_Commerce.Application.DTOs;
using E_Commerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            // Create your object-object mappings here
            CreateMap<RegisterDto, User>().ForMember(des=>des.HashedPassword ,otc=>otc.MapFrom(src=>src.Password));
            CreateMap<User, UserDto>();
        }
    }
}
