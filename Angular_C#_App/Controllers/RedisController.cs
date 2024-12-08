using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Shop_App.Controllers
{
    public class RedisController(RedisService redisService) : BaseApiController
    {

        [HttpGet("set")]
        public async Task<IActionResult> Set(string key, string value)
        {
            await redisService.SetValue(key, value);
            return Ok($"Key '{key}' set with value '{value}'");
        }

        [HttpGet("get")]
        public async Task<IActionResult> Get(string key)
        {
            var value = await redisService.GetValue(key);
            return Ok(new { Key = key, Value = value });
        }
    }
}
