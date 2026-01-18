using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.DTOs.Basket
{
    public class BasketItemDto
    {
        [Required]
        string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0.01,double.MaxValue)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        private int quantity;
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value<=0?1:value; }
        }
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public List<string> ImagesUrl { get; set; }
    }
}
