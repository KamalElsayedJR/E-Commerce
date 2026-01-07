using E_Commerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Models
{
    public class User: BaseModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public List<UserRoles> Roles { get; set; } = new List<UserRoles> { UserRoles.Customer};
        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
