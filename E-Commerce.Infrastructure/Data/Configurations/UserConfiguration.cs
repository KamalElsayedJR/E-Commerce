using E_Commerce.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(u => u.Email).IsRequired();
            builder.Property(u => u.FullName).IsRequired();
            builder.Property(u => u.HashedPassword).IsRequired();
            builder.Property(u => u.CreatedAt).IsRequired();
            builder.Property(u => u.UpdatedAt).IsRequired(false);
            builder.Property(u => u.Roles).IsRequired();

        }
    }
}
