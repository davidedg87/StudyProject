using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace YourNamespace.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class RedisController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;

        public RedisController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet("set")]
        public async Task<IActionResult> SetCache(string key, string value)
        {
            var options = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(1)); // Imposta il tempo di scadenza della cache

            // Imposta un valore nella cache
            await _distributedCache.SetStringAsync(key, value, options);
            return Ok($"Valore impostato per la chiave '{key}': '{value}'");
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetCache(string key)
        {
            // Recupera un valore dalla cache
            var value = await _distributedCache.GetStringAsync(key);
            if (value == null)
            {
                return NotFound($"Chiave '{key}' non trovata nella cache.");
            }
            return Ok($"Valore recuperato per la chiave '{key}': '{value}'");
        }

        [HttpGet("remove")]
        public async Task<IActionResult> RemoveCache(string key)
        {
            // Rimuovi un valore dalla cache
            await _distributedCache.RemoveAsync(key);
            return Ok($"Chiave '{key}' rimossa dalla cache.");
        }
    }
}
