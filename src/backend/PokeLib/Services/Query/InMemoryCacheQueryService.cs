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
    public sealed class InMemoryCacheQueryService : BaseQueryService, IInMemoryCacheQueryService
    {
        private readonly IInMemoryCache inMemoryCache;

        public InMemoryCacheQueryService(
            ILogger<InMemoryCacheQueryService> logger, 
            IInMemoryCache inMemoryCache, 
            IConfiguration configuration, 
            PokeApiClient client
            ) : base(logger, configuration, client)
        {
            this.inMemoryCache = inMemoryCache;

            #pragma warning disable CA2253
            this.logger.LogInformation("In Memory Cache Instance {0} Loaded With {1} Objects.",
                this.inMemoryCache.InstanceId, this.inMemoryCache.Cache.Count);
            #pragma warning restore CA2253
        }

        public override async Task<IEnumerable<NamedCachedResource>> QueryAsync(QueryOptions json)
        {
            IEnumerable<NamedCachedResource> results = Enumerable.Empty<NamedCachedResource>();
            string query = base.GetSanitizedQuery(json.Query);

            if (string.IsNullOrEmpty(query))
                return results;

            results = this.inMemoryCache.Cache
                .Where(x => x.Name.Contains(query))
                //.Where(x => x.Name.IndexOfMany(queryValues) < 4)
                .OrderBy(x => x.Name.IndexOfMany(query))
                .Take(base.MAX_RESULT_SIZE);

            if (!results.Any())
                return results;

            results = base.FilterByTypeOptions(results, json);

            //if (queryResult.First().Name.ToLower().Replace(' ', '-') == sanitizedQuery)
            //{
            //    queryResult = new List<CachedResource>
            //    {
            //        queryResult.FirstOrDefault(x => x.Name.Contains(sanitizedQuery))
            //    };
            //}

            foreach (NamedCachedResource result in results)
            {
                result.Json = await base.GetFullNamedResourceFromPokeApiAsync(result);
            }

            return results;
        }
    }
}
