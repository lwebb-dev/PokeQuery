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
                this.LoadResourceFileIntoCache(resourceType);
            }
        }

        public override void LoadResourceFileIntoCache(string resourceType)
        {
            string absoluteResourceDir = $"{base.CACHE_DIRECTORY}\\{resourceType}";
            string absoluteFilepath = $"{absoluteResourceDir}{base.FILE_EXTENSION}";
            if (!File.Exists(absoluteFilepath))
                return;

            string[] lines = File.ReadAllLines(absoluteFilepath);
            CachedResource resource;

            foreach (string line in lines)
            {
                resource = JsonSerializer.Deserialize<CachedResource>(line);
                resource.Json = File.ReadAllText($"{absoluteResourceDir}/{resource.Name}{base.FILE_EXTENSION}");
                this.Db.StringSet(resource.Name, JsonSerializer.Serialize(resource));
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
