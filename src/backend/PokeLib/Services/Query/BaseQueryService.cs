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

        public abstract Task<IEnumerable<NamedCachedResource>> QueryAsync(QueryOptions json);

        public async Task<string> QueryJsonAsync(QueryOptions json)
        {
            IEnumerable<NamedCachedResource> result = await this.QueryAsync(json);
            return JsonSerializer.Serialize(result);
        }

        public async Task<string> GetNamedPokeApiJsonResultAsync<T>(NamedCachedResource cachedResource)
            where T : NamedApiResource
        {
            string[] splitUri = cachedResource.Url.Split("/");
            int id = int.Parse(splitUri[^2]);
            T result = await this.client.GetResourceAsync<T>(id);
            this.logger.LogInformation("Request made to PokeApi on {0} at {1}", DateTime.UtcNow, cachedResource.Url);
            cachedResource.Json = JsonSerializer.Serialize(result);
            string absoluteJsonFilePath = $"{this.CACHE_DIRECTORY}/{cachedResource.NamedResourceType}/{cachedResource.Name}{FILE_EXTENSION}";
            await File.WriteAllTextAsync(absoluteJsonFilePath, cachedResource.Json);
            return cachedResource.Json;
        }

        public async Task<string> GetPokeApiJsonResultAsync<T>(CachedResource cachedResource)
            where T : ApiResource
        {
            string[] splitUri = cachedResource.Url.Split("/");
            int id = int.Parse(splitUri[^2]);
            T result = await this.client.GetResourceAsync<T>(id);
            this.logger.LogInformation("Request made to PokeApi on {0} at {1}", DateTime.UtcNow, cachedResource.Url);
            cachedResource.Json = JsonSerializer.Serialize(result);
            string absoluteJsonFilePath = $"{this.CACHE_DIRECTORY}/{cachedResource.ResourceType}/{cachedResource.ResourceType}_{cachedResource.Id}{FILE_EXTENSION}";
            await File.WriteAllTextAsync(absoluteJsonFilePath, cachedResource.Json);
            return cachedResource.Json;

        }

        internal string GetSanitizedQuery(string query)
        {
            if (string.IsNullOrEmpty(query) || query.Length < 3)
                return string.Empty;

            return query.ToLower().Replace(' ', '-');
        }

        internal IList<NamedCachedResource> FilterByTypeOptions(IEnumerable<NamedCachedResource> cachedResources, QueryOptions options)
        {
            List<NamedCachedResource> filteredResults = new List<NamedCachedResource>();

            foreach (NamedCachedResource cachedResource in cachedResources)
            {
                if (cachedResource.NamedResourceType == NamedResourceTypes.Pokemon && options.IncludePokemon)
                {
                    filteredResults.Add(cachedResource);
                    continue;
                }

                if (cachedResource.NamedResourceType == NamedResourceTypes.Items && options.IncludeItems)
                {
                    filteredResults.Add(cachedResource);
                    continue;
                }

                if (cachedResource.NamedResourceType == NamedResourceTypes.Moves && options.IncludeMoves)
                {
                    filteredResults.Add(cachedResource);
                    continue;
                }
            }

            return filteredResults;
        }

        internal async Task<string> GetFullNamedResourceFromPokeApiAsync(NamedCachedResource resource)
        {
            if (!string.IsNullOrEmpty(resource.Json))
                return resource.Json;

            // TODO: use reflection to call these programmatically
            if (resource.NamedResourceType == NamedResourceTypes.Pokemon)
            {
                return await this.GetNamedPokeApiJsonResultAsync<Pokemon>(resource);
            }

            if (resource.NamedResourceType == NamedResourceTypes.Items)
                return await this.GetNamedPokeApiJsonResultAsync<Item>(resource);

            if (resource.NamedResourceType == NamedResourceTypes.Moves)
                return await this.GetNamedPokeApiJsonResultAsync<Move>(resource);

            if (resource.NamedResourceType == NamedResourceTypes.Types)
                return await this.GetNamedPokeApiJsonResultAsync<PokeApiNet.Type>(resource);

            if (resource.NamedResourceType == NamedResourceTypes.VersionGroups)
                return await this.GetNamedPokeApiJsonResultAsync<VersionGroup>(resource);

            if (resource.NamedResourceType == NamedResourceTypes.Generations)
                return await this.GetNamedPokeApiJsonResultAsync<Generation>(resource);

            throw new NotImplementedException("Unknown NamedResource Type.");
        }

        internal async Task<string> GetFullApiResourceFromPokeApiAsync(CachedResource resource)
        {
            if (!string.IsNullOrEmpty(resource.Json))
                return resource.Json;

            if (resource.ResourceType == ResourceTypes.Machines)
                return await this.GetPokeApiJsonResultAsync<Machine>(resource);

            throw new NotImplementedException("Unknown Resource Type.");
        }
    }
}
