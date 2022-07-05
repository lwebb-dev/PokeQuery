using Microsoft.Extensions.Logging;
using PokeApiNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokeLib.Services
{
    public abstract class BaseQueryService : IBaseQueryService
    {
        internal readonly ILogger<BaseQueryService> logger;
        internal readonly PokeApiClient client;
        internal readonly string CACHE_DIRECTORY;
        internal readonly byte MAX_RESULT_SIZE;
        internal readonly IPokeCache pokeCache;

        public BaseQueryService(ILogger<BaseQueryService> logger, IPokeCache pokeCache)
        {
            this.logger = logger;
            this.CACHE_DIRECTORY = Environment.GetEnvironmentVariable("CACHE_DIRECTORY");
            this.MAX_RESULT_SIZE = byte.Parse(Environment.GetEnvironmentVariable("MAX_RESULT_SIZE"));
            this.client = new PokeApiClient();
            this.pokeCache = pokeCache;
        }

        public abstract Task<IEnumerable<CachedResource>> QueryAsync(string query = "");

        public async Task<string> QueryJsonAsync(string query = "")
        {
            IEnumerable<CachedResource> result = await this.QueryAsync(query);
            return JsonSerializer.Serialize(result);
        }

        internal abstract Task<string> GetPokeApiJsonResult<T>(CachedResource cachedResource)
            where T : NamedApiResource;

    }
}
