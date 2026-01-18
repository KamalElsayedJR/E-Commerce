using E_Commerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Models.OrderAggregate
{
    public class Order : BaseModel
    {
        public Order()
        {
            
        }

        public Order(string sellerEmail, Address shipToAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subtotal)
        {
            BuyerEmail = sellerEmail;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            Subtotal = subtotal;
        }
        public string BuyerEmail { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShipToAddress { get; set; }
        public string DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        public decimal Subtotal { get; set; }
        public string PaymentIntedntId { get; set; }
        public decimal GetTotal()
        => Subtotal + DeliveryMethod.Cost;
        
    }
}
