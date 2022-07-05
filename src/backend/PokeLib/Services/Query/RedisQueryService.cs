using Microsoft.Extensions.Logging;
using PokeLib.Cache;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeLib.Services
{
    public class RedisQueryService : BaseQueryService, IRedisQueryService
    {
        private readonly IRedisCache redisCache;
        public RedisQueryService(ILogger<BaseQueryService> logger, IRedisCache redisCache) 
            : base(logger)
        {
            this.redisCache = redisCache;

#pragma warning disable CA2253
            this.logger.LogInformation("Redis Cache Instance {0} Loaded With {1} Objects.",
                this.redisCache.InstanceId, this.redisCache.GetKeyCount());
#pragma warning restore CA2253

        }

        public override async Task<IEnumerable<CachedResource>> QueryAsync(string query = "")
        {
            IList<CachedResource> result = new List<CachedResource>();
            string[] queryValues = GetQueryValues(query);

            if (queryValues.Length <= 0)
                return result;

           foreach (string queryValue in queryValues)
            {
                result.Add(await this.redisCache.GetCachedResourceAsync(queryValue));
            }

           return result;
        }
    }
}
