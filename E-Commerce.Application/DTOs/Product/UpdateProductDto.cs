using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Product
{
    public class UpdateProductDto
    {
        public string? Name { get; set; }
        public List<IFormFile>? Images { get; set; }
        public string? Description { get; set; }
        [DataType(DataType.Currency)]
        public decimal? Price { get; set; }
        public int? Stock { get; set; } = 1;
        public string? CategoryId { get; set; }
    }
}
