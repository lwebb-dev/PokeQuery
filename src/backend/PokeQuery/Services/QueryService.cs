using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokeLib;
using PokeLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokeQuery.Services
{
    public class QueryService : IQueryService
    {
        private readonly ILogger<QueryService> logger;
        private readonly PokeApiClient client;
        private readonly string CACHE_DIRECTORY;
        private readonly byte MAX_RESULT_SIZE;
        private readonly IPokeCache pokeCache;

        public QueryService(ILogger<QueryService> logger, IPokeCache pokeCache)
        {
            this.logger = logger;
            this.CACHE_DIRECTORY = Environment.GetEnvironmentVariable("CACHE_DIRECTORY");
            this.MAX_RESULT_SIZE = byte.Parse(Environment.GetEnvironmentVariable("MAX_RESULT_SIZE"));
            this.client = new PokeApiClient();
            this.pokeCache = pokeCache;
            this.pokeCache.LoadResourceFileIntoCache($"{this.CACHE_DIRECTORY}\\pokemon.txt");
            this.pokeCache.LoadResourceFileIntoCache($"{this.CACHE_DIRECTORY}\\moves.txt");
            this.pokeCache.LoadResourceFileIntoCache($"{this.CACHE_DIRECTORY}\\items.txt");

#pragma warning disable CA2253
            this.logger.LogInformation("PokeCache Instance {0} Loaded With {1} Objects.", this.pokeCache.InstanceId, this.pokeCache.Cache.Count);
#pragma warning restore CA2253
        }

        public async Task<IEnumerable<CachedResource>> QueryAsync(string query = "")
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

            foreach (CachedResource item in result)
            {
                if (string.IsNullOrEmpty(item.Json))
                {
                    switch (item.ResourceType)
                    {
                        case "pokemon":
                            item.Json = await this.GetPokeApiJsonResult<Pokemon>(item);
                            break;
                        case "items":
                            item.Json = await this.GetPokeApiJsonResult<Item>(item);
                            break;
                        case "moves":
                            item.Json = await this.GetPokeApiJsonResult<Move>(item);
                            break;
                    }
                }
            }

            return result;
        }

        public async Task<string> QueryJsonAsync(string query = "")
        {
            IEnumerable<CachedResource> result = await this.QueryAsync(query);
            return JsonSerializer.Serialize(result);
        }

        private async Task<string> GetPokeApiJsonResult<T>(CachedResource cachedResource)
            where T : NamedApiResource
        {
            string[] splitUri = cachedResource.Url.Split("/");
            int id = int.Parse(splitUri[^2]);
            T result = await this.client.GetResourceAsync<T>(id);
            return JsonSerializer.Serialize(result);
        }

    }
}
