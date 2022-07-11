using Microsoft.Extensions.Logging;
using PokeLib.Cache;
using System;

namespace PokeLib.Utilities
{
    public static class RedisLoaderUtility
    {
        public static void LoadResourceFilesIntoCache(ILogger logger, IRedisCache redis)
        {
            int count = 0;

            foreach (ResourceTypes resourceType in Enum.GetValues(typeof(ResourceTypes)))
            {
                count += redis.LoadResourceFileIntoCache(resourceType);
            }

#pragma warning disable CA2253
            logger.LogInformation("Redis Cache Loaded With {0} Objects.", count);
#pragma warning restore CA2253
        }
    }
}
