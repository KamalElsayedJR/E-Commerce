using E_Commerce.Application.DTOs.User;
using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Models.OrderAggregate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Order
{
    public class OrderDto
    {
        [Required]
        public string BuyerEmail { get; set; }
        [Required]
        public OrderStatus Status { get; set; }
        [Required]
        public AddressDto ShipToAddress { get; set; }
        [Required]
        public string DeliveryMethodName { get; set; }
        [Required]
        public ICollection<OrderItemDto> Items { get; set; }
        [DataType(DataType.Currency)]
        [Required]
        public decimal Subtotal { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public string Total { get; set; }
        [Required]
        public string PaymentIntedntId { get; set; }
    }
}
