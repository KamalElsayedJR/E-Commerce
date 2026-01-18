using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Models.OrderAggregate
{
    public class OrderItem:BaseModel
    {
        public OrderItem()
        {
            
        }
        public OrderItem(string productId, string productName, List<string> imageUrl, decimal price, int quantity,  Order order)
        {
            ProductId = productId;
            ProductName = productName;
            ImageUrl = imageUrl;
            Price = price;
            Quantity = quantity;
            Order = order;
        }

        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public List<string> ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string OrderId { get; set; }
        public Order Order { get; set; }
    }
}
