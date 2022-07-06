using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokeLib.Cache;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeLib.Services
{
    public class RedisQueryService : BaseQueryService, IRedisQueryService
    {
        private readonly IRedisCache redisCache;
        public RedisQueryService(ILogger<BaseQueryService> logger, IRedisCache redisCache, IConfiguration configuration, PokeApiClient client) 
            : base(logger, configuration, client)
        {
            this.redisCache = redisCache;

#pragma warning disable CA2253
            this.logger.LogInformation("Redis Cache Instance {0} Loaded With {1} Objects.",
                this.redisCache.InstanceId, this.redisCache.GetKeyCount());
#pragma warning restore CA2253

        }

        public override async Task<IEnumerable<CachedResource>> QueryAsync(string query = "")
        {
            IList<CachedResource> results = new List<CachedResource>();
            string[] queryValues = GetQueryValues(query);

            if (queryValues.Length <= 0)
                return results;

           foreach (string queryValue in queryValues)
           {
               results.Add(await this.redisCache.GetCachedResourceAsync(queryValue));
           }

            foreach (CachedResource result in results)
            {
                if (results.Count == 1 && results[0] == null)
                    break;

                if (string.IsNullOrEmpty(result.Json))
                {
                    switch (result.ResourceType)
                    {
                        case "pokemon":
                            result.Json = await base.GetPokeApiJsonResultAsync<Pokemon>(result);
                            break;
                        case "items":
                            result.Json = await base.GetPokeApiJsonResultAsync<Item>(result);
                            break;
                        case "moves":
                            result.Json = await base.GetPokeApiJsonResultAsync<Move>(result);
                            break;
                    }

                    this.redisCache.UpdateCachedResource(result);
                }
            }

            return results;
        }
    }
}
