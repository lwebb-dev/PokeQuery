using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokeLib.Cache;
using PokeLib.Models;
using System.Collections.Generic;
using System.Linq;
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
        }

        public override async Task<IEnumerable<CachedResource>> QueryAsync(QueryOptions json)
        {
            IEnumerable<CachedResource> results = Enumerable.Empty<CachedResource>();
            string query = base.GetSanitizedQuery(json.Query);

            if (string.IsNullOrEmpty(query))
                return results;

            results = await this.redisCache.GetCachedResourcesByPatternAsync($"*{query}*");
            results = base.FilterByTypeOptions(results, json)
                .OrderBy(x => x.ResourceType)
                .Take(base.MAX_RESULT_SIZE);

            foreach (CachedResource result in results)
            {
                if (results.Count() == 1 && results.First() == null)
                    break;

                result.Json = await base.GetFullResourceFromPokeApiAsync(result);
                this.redisCache.UpdateCachedResource(result);
            }

            return results;
        }
    }
}
