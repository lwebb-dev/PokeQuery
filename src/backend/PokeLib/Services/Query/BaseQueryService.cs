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

        internal IList<CachedResource> FilterByTypeOptions(IEnumerable<CachedResource> cachedResources, QueryOptions options)
        {
            List<CachedResource> filteredResults = new List<CachedResource>();

            foreach (CachedResource cachedResource in cachedResources)
            {
                if (cachedResource.ResourceType == ResourceTypes.Pokemon && options.IncludePokemon)
                {
                    filteredResults.Add(cachedResource);
                    continue;
                }

                if (cachedResource.ResourceType == ResourceTypes.Items && options.IncludeItems)
                {
                    filteredResults.Add(cachedResource);
                    continue;
                }

                if (cachedResource.ResourceType == ResourceTypes.Moves && options.IncludeMoves)
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

            if (resource.ResourceType == ResourceTypes.Pokemon)
            {
                return await this.GetPokeApiJsonResultAsync<Pokemon>(resource);
            }

            if (resource.ResourceType == ResourceTypes.Items)
                return await this.GetPokeApiJsonResultAsync<Item>(resource);

            if (resource.ResourceType == ResourceTypes.Moves)
                return await this.GetPokeApiJsonResultAsync<Move>(resource);

            if (resource.ResourceType == ResourceTypes.Types)
                return await this.GetPokeApiJsonResultAsync<PokeApiNet.Type>(resource);

            if (resource.ResourceType == ResourceTypes.VersionGroups)
                return await this.GetPokeApiJsonResultAsync<VersionGroup>(resource);

            throw new NotImplementedException("Unknown Resource Type.");
        }
    }
}
