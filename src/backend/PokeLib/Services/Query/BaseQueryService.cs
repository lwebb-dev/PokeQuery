using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using PokeLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokeLib.Services
{
    public abstract class BaseQueryService : IBaseQueryService
    {
        internal readonly ILogger<BaseQueryService> logger;
        internal readonly IConfiguration configuration;
        internal readonly PokeApiClient client;

        internal byte MAX_RESULT_SIZE => byte.Parse(this.configuration["MAX_RESULT_SIZE"]);
        private string CACHE_DIRECTORY => this.configuration["CACHE_DIRECTORY"];
        private string FILE_EXTENSION => ".txt";


        public BaseQueryService(ILogger<BaseQueryService> logger, IConfiguration configuration, PokeApiClient client)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.client = client;
        }

        public abstract Task<IEnumerable<CachedResource>> QueryAsync(QueryOptions json);

        public async Task<string> QueryJsonAsync(QueryOptions json)
        {
            IEnumerable<CachedResource> result = await this.QueryAsync(json);
            return JsonSerializer.Serialize(result);
        }

        public async Task<string> GetPokeApiJsonResultAsync<T>(CachedResource cachedResource)
            where T : NamedApiResource
        {
            string[] splitUri = cachedResource.Url.Split("/");
            int id = int.Parse(splitUri[^2]);
            T result = await this.client.GetResourceAsync<T>(id);
            this.logger.LogInformation("Request made to PokeApi on {0} at {1}", DateTime.UtcNow, cachedResource.Url);
            cachedResource.Json = JsonSerializer.Serialize(result);
            string absoluteJsonFilePath = $"{this.CACHE_DIRECTORY}/{cachedResource.ResourceType}/{cachedResource.Name}{FILE_EXTENSION}";
            await File.WriteAllTextAsync(absoluteJsonFilePath, cachedResource.Json);
            return cachedResource.Json;
        }

        internal string GetSanitizedQuery(string query)
        {
            if (string.IsNullOrEmpty(query) || query.Length < 3)
                return string.Empty;

            return query.ToLower().Replace(' ', '-');
        }

        internal IList<CachedResource> FilterByTypeOptions(IList<CachedResource> cachedResources, QueryOptions options)
        {
            List<CachedResource> filteredResults = new List<CachedResource>();

            foreach (CachedResource cachedResource in cachedResources)
            {
                if (cachedResource.ResourceType == "pokemon" && options.IncludePokemon)
                {
                    filteredResults.Add(cachedResource);
                    continue;
                }

                if (cachedResource.ResourceType == "item" && options.IncludeItems)
                {
                    filteredResults.Add(cachedResource);
                    continue;
                }

                if (cachedResource.ResourceType == "move" && options.IncludeMoves)
                {
                    filteredResults.Add(cachedResource);
                    continue;
                }
            }

            return filteredResults;
        }

        internal async Task<string> GetFullResourceFromPokeApiAsync(CachedResource resource)
        {
            if (!string.IsNullOrEmpty(resource.Json))
                return resource.Json;

            if (resource.ResourceType == "pokemon")
            {
                return await this.GetPokeApiJsonResultAsync<Pokemon>(resource);
            }

            if (resource.ResourceType == "items")
                return await this.GetPokeApiJsonResultAsync<Item>(resource);

            if (resource.ResourceType == "moves")
                return await this.GetPokeApiJsonResultAsync<Move>(resource);

            throw new NotImplementedException("Unknown Resource Type.");
        }
    }
}
