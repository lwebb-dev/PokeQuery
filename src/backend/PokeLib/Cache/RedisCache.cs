using StackExchange.Redis;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokeLib.Cache
{
    internal class RedisCache : BaseCache, IRedisCache
    {
        private IConnectionMultiplexer Redis { get; set; }
        private IDatabase Db { get; set; }

        public RedisCache(string cacheDirectory, string redisConnectionString)
            : base(cacheDirectory)
        {
            this.Redis = ConnectionMultiplexer.Connect(redisConnectionString);
            this.Db = this.Redis.GetDatabase();

            foreach (string resourceType in base.RESOURCE_TYPES)
            {
                this.LoadResourceFileIntoCache($"{base.CACHE_DIRECTORY}\\{resourceType}{base.FILE_EXTENSION}");
            }
        }

        public override void LoadResourceFileIntoCache(string fileDirectory)
        {
            if (!File.Exists(fileDirectory))
                return;

            string[] lines = File.ReadAllLines(fileDirectory);
            CachedResource resource;

            foreach (string line in lines)
            {
                resource = JsonSerializer.Deserialize<CachedResource>(line);
                this.Db.StringSet(resource.Name, line);
            }

            return;
        }

        public int GetKeyCount()
        {
            IServer server = this.Redis.GetServer(this.Redis.GetEndPoints().First());
            return server.Keys(pattern: "*").Count();
        }

        public async Task<CachedResource> GetCachedResourceAsync(string key)
        {
            string result = await this.Db.StringGetAsync(key);

            if (string.IsNullOrEmpty(result))
                return null;

            return JsonSerializer.Deserialize<CachedResource>(result);
        }
    }
}
