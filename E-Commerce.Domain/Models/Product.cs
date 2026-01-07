using E_Commerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Models
{
    public class Product:BaseModel
    {
        public string Name { get; set; }
        public List<string> ImagesUrl { get; set; } = new();
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public ProductStatus Status { get; set; } = ProductStatus.Pending;
        public string CategoryId { get; set; }
        public Category Category { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
