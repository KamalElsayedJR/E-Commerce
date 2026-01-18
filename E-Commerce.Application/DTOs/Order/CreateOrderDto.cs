using E_Commerce.Application.DTOs.User;
using E_Commerce.Domain.Models.OrderAggregate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Order
{
    public class CreateOrderDto
    {
        [Required]
        public string BasketId { get; set; }
        [Required]
        public string DeliveryMethodId { get; set; }
        [Required]
        public AddressDto ShippingAddress { get; set; }
    }
}
