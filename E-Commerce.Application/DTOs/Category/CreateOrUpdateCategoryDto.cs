using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Category
{
    public class CreateOrUpdateCategoryDto
    {
        [Required]
        public string Name { get; set; }
        [Required]

        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
