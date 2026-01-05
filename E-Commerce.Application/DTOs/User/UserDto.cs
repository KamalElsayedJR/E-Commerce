using E_Commerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.User
{
    public class UserDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<UserRoles> Roles { get; set; }
       
        
    }
}
