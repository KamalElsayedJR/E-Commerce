using AutoMapper.Execution;
using E_Commerce.Domain.Enums;
using E_Commerce.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Data.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            var rolesConverter = new ValueConverter<List<UserRoles>, string>(
                                                                       v => string.Join(',', v),
                                                                       v => v.Split(',',StringSplitOptions.RemoveEmptyEntries)
                                                                             .Select(r => Enum.Parse<UserRoles>(r))
                                                                             .ToList()
                                                                             );
            var rolesComparer = new ValueComparer<List<UserRoles>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()
            );
            builder.HasKey(u => u.Id);
            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(u => u.Email).IsRequired();
            builder.Property(u => u.HashedPassword).IsRequired();
            builder.Property(u => u.CreatedAt).IsRequired();
            builder.Property(u => u.Roles).IsRequired();
            builder.Property(u => u.Roles)
                    .HasConversion(rolesConverter)
                    .Metadata
                    .SetValueComparer(rolesComparer);
        }
    }
}
