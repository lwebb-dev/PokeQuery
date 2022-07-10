using StackExchange.Redis;
using System.Collections.Generic;
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
            return this.GetKeys("*").Count();
        }

        public async Task<IEnumerable<CachedResource>> GetCachedResourcesByPatternAsync(string pattern)
        {
            IEnumerable<RedisKey> keys = this.GetKeys(pattern);
            IList<CachedResource> results = new List<CachedResource>();
            string keyString = string.Empty;

            foreach (RedisKey key in keys)
            {
                keyString = key.ToString();

                if (key.ToString() == "(null)")
                    continue;

                results.Add(await this.GetCachedResourceAsync(keyString));
            }

            return results;
        }

        public async Task<CachedResource> GetCachedResourceAsync(string key)
        {
            string result = await this.Db.StringGetAsync(key);

            if (string.IsNullOrEmpty(result))
                return null;

            return JsonSerializer.Deserialize<CachedResource>(result);
        }

        public bool UpdateCachedResource(CachedResource cachedResource)
        {
            return this.Db.StringSet(cachedResource.Name, JsonSerializer.Serialize(cachedResource));
        }

        private IEnumerable<RedisKey> GetKeys(string pattern)
        {
            IServer server = this.Redis.GetServer(this.Redis.GetEndPoints().First());
            return server.Keys(pattern: pattern);
        }
    }
}
