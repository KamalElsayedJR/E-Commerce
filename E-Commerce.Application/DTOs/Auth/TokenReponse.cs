using E_Commerce.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Auth
{
    public class TokenReponse
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}
