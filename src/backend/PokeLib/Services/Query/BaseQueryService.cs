using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PokeApiNet;
using System;
using System.Collections.Generic;
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

        public BaseQueryService(ILogger<BaseQueryService> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
            this.client = new PokeApiClient();
        }

        public abstract Task<IEnumerable<CachedResource>> QueryAsync(string query = "");

        public async Task<string> QueryJsonAsync(string query = "")
        {
            IEnumerable<CachedResource> result = await this.QueryAsync(query);
            return JsonSerializer.Serialize(result);
        }

        public async Task<string> GetPokeApiJsonResult<T>(CachedResource cachedResource)
            where T : NamedApiResource
        {
            string[] splitUri = cachedResource.Url.Split("/");
            int id = int.Parse(splitUri[^2]);
            T result = await this.client.GetResourceAsync<T>(id);
            return JsonSerializer.Serialize(result);
        }


        internal static string[] GetQueryValues(string query)
        {
            if (string.IsNullOrEmpty(query) || query.Length < 3)
                return Array.Empty<string>();

            string sanitizedQuery = query.ToLower().Replace(' ', '-');
            return sanitizedQuery.Split(' ');
        }

    }
}
