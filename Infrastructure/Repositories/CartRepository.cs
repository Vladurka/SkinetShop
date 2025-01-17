using Core.Enities;
using Core.Enities.Service.Contracts;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Repositories;

public class CartRepository(IConnectionMultiplexer redis) : ICartRepository
{
    private readonly IDatabase _database = redis.GetDatabase();
    public async Task<ShoppingCart?> GetCartAsync(string key)
    {
        var data = await _database.StringGetAsync(key);

        return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<ShoppingCart>(data!);
    }

    public async Task<ShoppingCart?> SetCartAsync(ShoppingCart cart)
    {
        var created = await _database.StringSetAsync(cart.Id.ToString(),
        JsonSerializer.Serialize(cart), TimeSpan.FromDays(30));

        return created ? await GetCartAsync(cart.Id.ToString()) : null;
    }

    public async Task<bool> DeleteCartAsync(string key) =>
        await _database.KeyDeleteAsync(key);
}
