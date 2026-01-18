using E_Commerce.Application.Interfaces.Services;
using E_Commerce.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.implementation
{
    public class TokenServices : ITokenServices
    {
        private readonly IConfiguration _configuration;

        public TokenServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> GenerateAccessTokenAsync(User user)
        {
            var AuthClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.FullName)
            };
            foreach(var role in user.Roles)
            {
                AuthClaims.Add(new Claim(ClaimTypes.Role,role.ToString()));
            }
            var Securekey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecureKey"]));
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JWT:ExpiryInMin"])),
                signingCredentials: new SigningCredentials(Securekey,SecurityAlgorithms.HmacSha256Signature),
                claims: AuthClaims
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<string> GenerateRereshTokenAsync()
        {
            var randomNmber = new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randomNmber);
            return (Convert.ToBase64String(randomNmber));
        }
    }
}
