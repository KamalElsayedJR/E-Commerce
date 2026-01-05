using E_Commerce.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Auth
{
    public class AuthDtoResponse
    {
        public string AccessToken { get; set; }
        public DateTime RefreshTokenExpierAt { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }
        public UserDto User { get; set; }
    }
}
