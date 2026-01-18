using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.User
{
    public class UserDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public List<UserRoles> Roles { get; set; }
        [Required]
        public AddressDto Address { get; set; }
    }
}
