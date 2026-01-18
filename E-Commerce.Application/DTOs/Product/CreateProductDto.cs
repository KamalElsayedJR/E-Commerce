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
        [DataType(DataType.Currency)]

        public decimal Price { get; set; }
        [Required]
        private int stock;
        public int Stock
        {
            get { return stock; }
            set { stock = value<=0?1:value; }
        }
        [Required]
        public string CategoryId { get; set; }
    }
}
