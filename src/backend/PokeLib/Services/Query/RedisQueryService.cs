using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokeLib.Cache;
using PokeLib.Models;
using System;
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
            string query = base.GetSanitizedQuery(json.Query.Replace("_", ""));

            if (string.IsNullOrEmpty(query))
                return results;

            results = await this.redisCache.GetCachedNamedResourcesByPatternAsync($"*{query}*");
            results = base.FilterByTypeOptions(results, json)
                .Where(x => x.Id < 10000) // Exclude Forms
                .OrderBy(x => x.SortIndex)
                .Take(base.MAX_RESULT_SIZE);

            await this.CheckUpdateNamedResourceJson(results);
            return results;
        }

        public async Task<IEnumerable<T>> GetNamedResourceAsync<T>(NamedResourceTypes namedResourceType)
            where T : NamedApiResource
        {
            IEnumerable<NamedCachedResource> cacheResults = await this.redisCache.GetCachedNamedResourcesByPatternAsync("*");
            cacheResults = cacheResults.Where(x => x.NamedResourceType == namedResourceType);
            await this.CheckUpdateNamedResourceJson(cacheResults);

            return cacheResults.Select(x => JsonSerializer.Deserialize<T>(x.Json));
        }

        public async Task<IEnumerable<T>> GetApiResourceAsync<T>(ResourceTypes resourceType)
            where T : ApiResource
        {
            string resourceTypeName = Enum.GetName(typeof(ResourceTypes), resourceType).ToLower();
            IEnumerable<CachedResource> cacheResults = await this.redisCache.GetCachedResourcesByPatternAsync($"*_{resourceTypeName}*");
            await this.CheckUpdateApiResourceJson(cacheResults);

            return cacheResults
                .Where(x => !string.IsNullOrEmpty(x.Json))
                .Select(x => JsonSerializer.Deserialize<T>(x.Json));
        }

        private async Task CheckUpdateNamedResourceJson(IEnumerable<NamedCachedResource> results)
        {
            foreach (NamedCachedResource result in results)
            {
                if (results.Count() == 1 && results.First() == null)
                    break;

                result.Json = await base.GetFullNamedResourceFromPokeApiAsync(result);
                this.redisCache.UpdateNamedCachedResource(result);
            }
        }

        private async Task CheckUpdateApiResourceJson(IEnumerable<CachedResource> results, int throttle = 5)
        {
            int requestsMade = 0;

            foreach (CachedResource result in results)
            {
                if (results.Count() == 1 && results.First() == null)
                    break;

                if (!string.IsNullOrEmpty(result.Json))
                    continue;

                result.Json = await base.GetFullApiResourceFromPokeApiAsync(result);
                this.redisCache.UpdateCachedResource(result);
                requestsMade++;

                // do this so we don't blast PokeApi with hundreds of requests
                if (requestsMade >= throttle)
                    break;
            }
        }
    }

}
