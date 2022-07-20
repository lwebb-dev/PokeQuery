using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
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

            foreach (NamedResourceTypes namedResourceType in Enum.GetValues(typeof(NamedResourceTypes)))
            {
                this.LoadNamedResourceFileIntoCache(namedResourceType);
            }
        }

        public override int LoadNamedResourceFileIntoCache(NamedResourceTypes namedResourceType)
        {
            string resourceTypeName = Enum.GetName(typeof(NamedResourceTypes), namedResourceType).ToLower();
            string absoluteResourceDir = $"{base.CACHE_DIRECTORY}\\{resourceTypeName}";
            string absoluteFilepath = $"{absoluteResourceDir}{base.FILE_EXTENSION}";

            if (!File.Exists(absoluteFilepath))
                return 0;

            string[] lines = File.ReadAllLines(absoluteFilepath);
            NamedCachedResource resource;
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());

            foreach (string line in lines)
            {
                resource = JsonSerializer.Deserialize<NamedCachedResource>(line, options);
                resource.Json = File.ReadAllText($"{absoluteResourceDir}/{resource.Name}{base.FILE_EXTENSION}");
                this.Db.StringSet(resource.Name, JsonSerializer.Serialize(resource));
            }

            return lines.Length;
        }

        public int GetKeyCount()
        {
            return this.GetKeys("*").Count();
        }

        public async Task<IEnumerable<NamedCachedResource>> GetCachedResourcesByPatternAsync(string pattern)
        {
            IEnumerable<RedisKey> keys = this.GetKeys(pattern);
            IList<NamedCachedResource> results = new List<NamedCachedResource>();
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

        public async Task<NamedCachedResource> GetCachedResourceAsync(string key)
        {
            string result = await this.Db.StringGetAsync(key);

            if (string.IsNullOrEmpty(result))
                return null;

            return JsonSerializer.Deserialize<NamedCachedResource>(result);
        }

        public bool UpdateCachedResource(NamedCachedResource cachedResource)
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
