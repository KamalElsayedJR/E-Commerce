using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Product
{
    public class Pagination : ProductDto
    {
        [Required]
        public int PageNumber { get; set; } = 1;
        [Required]
        public int PageSize { get; set; }
        [Required]
        public int TotalCount { get; set; }
    }
}
