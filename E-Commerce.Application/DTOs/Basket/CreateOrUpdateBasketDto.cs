using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Basket
{
    public class CreateOrUpdateBasketDto
    {
        [Required]
        public List<BasketItemDto> Items { get; set; }
    }
}
