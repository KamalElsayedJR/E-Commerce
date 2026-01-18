using E_Commerce.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Auth
{
    public class AuthDtoResponse
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public DateTime RefreshTokenExpierAt { get; set; }
        [Required]
        [JsonIgnore]
        public string RefreshToken { get; set; }
        [Required]
        public UserDto User { get; set; }
    }
}
