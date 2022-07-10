using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokeLib.Cache;
using PokeLib.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeLib.Services
{
    public class RedisQueryService : BaseQueryService, IRedisQueryService
    {
        private readonly IRedisCache redisCache;
        public RedisQueryService(
            ILogger<BaseQueryService> logger, 
            IRedisCache redisCache, 
            IConfiguration configuration,
            PokeApiClient client
            ) : base(logger, configuration, client)
        {
            this.redisCache = redisCache;

#pragma warning disable CA2253
            this.logger.LogInformation("Redis Cache Instance {0} Loaded With {1} Objects.",
                this.redisCache.InstanceId, this.redisCache.GetKeyCount());
#pragma warning restore CA2253

        }

        public override async Task<IEnumerable<CachedResource>> QueryAsync(QueryOptions json)
        {
            IList<CachedResource> results = new List<CachedResource>();
            string query = base.GetSanitizedQuery(json.Query);

            if (string.IsNullOrEmpty(query))
                return results;

            results.Add(await this.redisCache.GetCachedResourceAsync(query));
            results = base.FilterByTypeOptions(results, json);

            foreach (CachedResource result in results)
            {
                if (results.Count == 1 && results[0] == null)
                    break;

                result.Json = await base.GetFullResourceFromPokeApiAsync(result);
                this.redisCache.UpdateCachedResource(result);
            }

            return results;
        }
    }
}
