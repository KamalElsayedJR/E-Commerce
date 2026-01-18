using E_Commerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Product
{
    public class ProductDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public List<string> ImagesUrl { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Required]
        public int Stock { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}
