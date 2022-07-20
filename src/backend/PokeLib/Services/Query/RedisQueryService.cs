using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokeLib.Cache;
using PokeLib.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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

        public override async Task<IEnumerable<NamedCachedResource>> QueryAsync(QueryOptions json)
        {
            IEnumerable<NamedCachedResource> results = Enumerable.Empty<NamedCachedResource>();
            string query = base.GetSanitizedQuery(json.Query);

            if (string.IsNullOrEmpty(query))
                return results;

            results = await this.redisCache.GetCachedResourcesByPatternAsync($"*{query}*");
            results = base.FilterByTypeOptions(results, json)
                .Where(x => x.Id < 10000) // Exclude Forms
                .OrderBy(x => x.SortIndex)
                .Take(base.MAX_RESULT_SIZE);

            await this.UpdateResultJsonIfNeeded(results);
            return results;
        }

        public async Task<IEnumerable<T>> GetResourceAsync<T>(NamedResourceTypes namedResourceType)
            where T : NamedApiResource
        {
            IEnumerable<NamedCachedResource> cacheResults = await this.redisCache.GetCachedResourcesByPatternAsync("*");
            cacheResults = cacheResults.Where(x => x.NamedResourceType == namedResourceType);
            await this.UpdateResultJsonIfNeeded(cacheResults);

            return cacheResults.Select(x => JsonSerializer.Deserialize<T>(x.Json));
        }

        private async Task UpdateResultJsonIfNeeded(IEnumerable<NamedCachedResource> results)
        {
            foreach (NamedCachedResource result in results)
            {
                if (results.Count() == 1 && results.First() == null)
                    break;

                result.Json = await base.GetFullNamedResourceFromPokeApiAsync(result);
                this.redisCache.UpdateCachedResource(result);
            }
        }
    }
}
