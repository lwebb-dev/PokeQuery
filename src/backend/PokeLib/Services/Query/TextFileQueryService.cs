using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokeLib.Services
{
    public sealed class TextFileQueryService : BaseQueryService, ITextFileQueryService
    {
        public TextFileQueryService(ILogger<BaseQueryService> logger, IPokeCache pokeCache) 
            : base(logger, pokeCache)
        {
            #pragma warning disable CA2253
            this.logger.LogInformation("PokeCache Instance {0} Loaded From Text Files With {1} Objects.",
                this.pokeCache.InstanceId, this.pokeCache.Cache.Count);
            #pragma warning restore CA2253
        }

        public override async Task<IEnumerable<CachedResource>> QueryAsync(string query = "")
        {
            IList<CachedResource> result = new List<CachedResource>();

            if (string.IsNullOrEmpty(query) || query.Length < 3)
                return result;


            string sanitizedQuery = query.ToLower().Replace(' ', '-');
            string[] queryValues = sanitizedQuery.Split(' ');
            result = this.pokeCache.Cache
                .Where(x => x.Name.ContainsAny(queryValues))
                //.Where(x => x.Name.IndexOfMany(queryValues) < 4)
                .OrderBy(x => x.Name.IndexOfMany(queryValues))
                .Take(this.MAX_RESULT_SIZE).ToList();

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

        internal override async Task<string> GetPokeApiJsonResult<T>(CachedResource cachedResource)
        {
            string[] splitUri = cachedResource.Url.Split("/");
            int id = int.Parse(splitUri[^2]);
            T result = await this.client.GetResourceAsync<T>(id);
            return JsonSerializer.Serialize(result);
        }

    }
}
