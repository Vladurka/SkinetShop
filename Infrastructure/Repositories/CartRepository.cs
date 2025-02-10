using Core.Enities;
using Core.Enities.Service.Contracts;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Repositories;

public class CartRepository(IDistributedCache redis) : ICartRepository
{
    public async Task<ShoppingCart?> GetCartAsync(string key)
    {
        string? data = await redis.GetStringAsync(key);

        return string.IsNullOrEmpty(data) ? null : JsonSerializer.Deserialize<ShoppingCart>(data);
    }

    public async Task<ShoppingCart?> SetCartAsync(ShoppingCart cart)
    {
        await redis.SetStringAsync(cart.Id.ToString(), JsonSerializer.Serialize(cart), new DistributedCacheEntryOptions
        { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) });

        return await GetCartAsync(cart.Id.ToString());
    }

    public async Task DeleteCartAsync(string key) =>
        await redis.RemoveAsync(key);
}
