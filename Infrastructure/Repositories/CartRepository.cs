using Core.Enities;
using Core.Enities.Service.Contracts;
using System.Text.Json;
using StackExchange.Redis;

namespace Infrastructure.Repositories;

public class CartRepository(IConnectionMultiplexer redis) : ICartRepository
{
    private readonly IDatabase _database = redis.GetDatabase();

    public async Task<bool> DeleteCartAsync(string key) =>
        await _database.KeyDeleteAsync(key);

    public async Task<ShoppingCart?> GetCartAsync(string key)
    {
        var data = await _database.StringGetAsync(key);

        return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<ShoppingCart>(data!);
    }

    public async Task<ShoppingCart?> SetCartAsync(ShoppingCart cart)
    {
        var created = await _database.StringSetAsync(cart.Id.ToString(),
            JsonSerializer.Serialize(cart), TimeSpan.FromDays(30));
    
        if (!created) return null;

        return await GetCartAsync(cart.Id.ToString());
    }
}
