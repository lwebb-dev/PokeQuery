using Microsoft.Extensions.Configuration;
using NRediSearch;
using NReJSON;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokeQuery.Services
{
    public class RedisService : IRedisService
    {
        private readonly IConnectionMultiplexer redis;
        private readonly IDatabase db;

        public RedisService(IConfiguration configuration)
        {
            this.redis = ConnectionMultiplexer.Connect(configuration["REDIS_CONNECTION"]);
            this.db = this.redis.GetDatabase();
        }

        public async Task<IEnumerable<string>> QueryIndexJsonAsync(string index, string query)
        {
            var searchClient = new Client(index, this.db);
            SearchResult searchResult = await searchClient.SearchAsync(new Query(query));
            return searchResult.Documents.Select(x => x["json"].ToString());
        }

        public async Task<IEnumerable<string>> GetJsonResultsByPatternAsync(string pattern)
        {
            IEnumerable<RedisKey> keys = this.GetKeys(pattern);
            var results = new List<string>();

            foreach (RedisKey key in keys)
            {
                string keyString = key.ToString();

                if (keyString == "(null)")
                    continue;

                results.Add(await this.GetJsonResultAsync(keyString));
            }

            return results;

        }

        private async Task<string> GetJsonResultAsync(string key)
        {
            RedisResult result = await this.db.JsonGetAsync(key);

            if (result.IsNull)
                return null;

            return result.ToString();
        }

        private IEnumerable<RedisKey> GetKeys(string pattern)
        {
            IServer server = this.redis.GetServer(this.redis.GetEndPoints().First());
            return server.Keys(pattern: pattern);
        }
    }
}
