using StackExchange.Redis;

namespace Infrastructure.Services;

public class RedisService
{
  private readonly IConnectionMultiplexer _redis;

  public RedisService(IConnectionMultiplexer redis)
  {
    _redis = redis;
  }

  public async Task SetValue(string key, string value)
  {
    var db = _redis.GetDatabase();
    await db.StringSetAsync(key, value);
  }

  public async Task<string> GetValue(string key)
  {
    var db = _redis.GetDatabase();
    var value = await db.StringGetAsync(key);
    return value.ToString();
  }
}
