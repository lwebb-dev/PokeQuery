using Microsoft.Extensions.Logging;
using PokeLib.Cache;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokeLib.Services
{
    public sealed class InMemoryCacheQueryService : BaseQueryService, IInMemoryCacheQueryService
    {
        private readonly IInMemoryCache inMemoryCache;

        public InMemoryCacheQueryService(ILogger<InMemoryCacheQueryService> logger, IInMemoryCache inMemoryCache) 
            : base(logger)
        {
            this.inMemoryCache = inMemoryCache;

            #pragma warning disable CA2253
            this.logger.LogInformation("In Memory Cache Instance {0} Loaded With {1} Objects.",
                this.inMemoryCache.InstanceId, this.inMemoryCache.Cache.Count);
            #pragma warning restore CA2253
        }

        public override async Task<IEnumerable<CachedResource>> QueryAsync(string query = "")
        {
            IList<CachedResource> result = new List<CachedResource>();
            string[] queryValues = GetQueryValues(query);

            if (queryValues.Length <= 0)
                return result;

            result = this.inMemoryCache.Cache
                .Where(x => x.Name.ContainsAny(queryValues))
                //.Where(x => x.Name.IndexOfMany(queryValues) < 4)
                .OrderBy(x => x.Name.IndexOfMany(queryValues))
                .Take(base.MAX_RESULT_SIZE).ToList();

            if (!result.Any())
                return result;

            //if (queryResult.First().Name.ToLower().Replace(' ', '-') == sanitizedQuery)
            //{
            //    queryResult = new List<CachedResource>
            //    {
            //        queryResult.FirstOrDefault(x => x.Name.Contains(sanitizedQuery))
            //    };
            //}

            //foreach (CachedResource item in result)
            //{
            //    if (string.IsNullOrEmpty(item.Json))
            //    {
            //        switch (item.ResourceType)
            //        {
            //            case "pokemon":
            //                item.Json = await this.GetPokeApiJsonResult<Pokemon>(item);
            //                break;
            //            case "items":
            //                item.Json = await this.GetPokeApiJsonResult<Item>(item);
            //                break;
            //            case "moves":
            //                item.Json = await this.GetPokeApiJsonResult<Move>(item);
            //                break;
            //        }
            //    }
            //}

            return result;
        }
    }
}
