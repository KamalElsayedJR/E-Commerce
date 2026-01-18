using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Models.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.BuyerEmail).IsRequired().HasMaxLength(100);
            builder.Property(o => o.Status).HasConversion(o=>o.ToString(),o=>(OrderStatus)Enum.Parse(typeof(OrderStatus),o));
            builder.Property(o=>o.Subtotal).HasColumnType("decimal(18,2)");
            builder.OwnsOne(O => O.ShipToAddress,A=>A.WithOwner());
        }
    }
}
