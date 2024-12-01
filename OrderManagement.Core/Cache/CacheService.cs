using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.Cache
{
    public interface ICacheService
    {
        Task<string?> GetCachedValueAsync(string key, bool useMemory = true);
        Task SetCachedValueAsync(string key, string value, TimeSpan expiration, bool useMemory = true);
    }

    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;

        public CacheService(IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
        }

        public async Task<string?> GetCachedValueAsync(string key, bool useMemory = true)
        {
            if (useMemory)
            {
                // Prima controlla la cache in memoria
                if (_memoryCache.TryGetValue(key, out string cachedValue))
                {
                    return cachedValue;
                }
                else
                    return null;
            }
            else
            {
                // Se non è presente in memoria, controlla Redis
                return await _distributedCache.GetStringAsync(key);
            }
        }

        public async Task SetCachedValueAsync(string key, string value, TimeSpan expiration, bool useMemory = true)
        {
            // Memorizza in memoria
            if (useMemory)
                _memoryCache.Set(key, value, expiration);
            else
            {
                // Memorizza anche in Redis
                await _distributedCache.SetStringAsync(key, value, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration
                });
            }
        }
    }



}
