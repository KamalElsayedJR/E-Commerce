using E_Commerce.Domain.Models.OrderAggregate;
using E_Commerce.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Hellper
{
    public class DataSeed
    {
        public static async Task SeedAsync(ECommerceDbContext dbContext)
        {
            if (!dbContext.DeliveryMethods.Any())
            {
                var delivery = File.ReadAllText("../E-Commerce.Infrastructure/Data/DataSeeding/delivery.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(delivery);
                if (deliveryMethods is not null)
                {
                    foreach (var dm in deliveryMethods)
                    {
                        await dbContext.DeliveryMethods.AddAsync(dm);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
        }

    }
}
