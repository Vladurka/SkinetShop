using Core.Enities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if (!await context.Products.AnyAsync())
            {
                var productsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");

                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if(products == null) return;

                context.Products.AddRange(products);

                await context.SaveChangesAsync();
            }
            
            if (!await context.DeliveryMethods.AnyAsync())
            {   
                var dmData = await File
                    .ReadAllTextAsync("../Infrastructure/Data/SeedData/delivery.json");

                var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);

                if (methods == null) return;

                context.DeliveryMethods.AddRange(methods);

                await context.SaveChangesAsync();
            }
        }
    }
}
