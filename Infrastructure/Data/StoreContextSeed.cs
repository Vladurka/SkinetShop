using Core.Enities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext storeContext)
        {
            if (!await storeContext.Products.AnyAsync())
            {
                var productsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");

                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if(products == null) return;

                storeContext.Products.AddRange(products);

                await storeContext.SaveChangesAsync();
            }
        }
    }
}
