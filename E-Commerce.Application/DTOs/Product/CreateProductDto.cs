using E_Commerce.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Product
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public List<IFormFile> Images{ get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }=1;
        [Required]
        public string CategoryId { get; set; }
    }
}
