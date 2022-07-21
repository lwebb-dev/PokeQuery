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

            foreach (ResourceTypes resourceType in Enum.GetValues(typeof(ResourceTypes)))
            {
                this.LoadApiResourceFileIntoCache(resourceType);
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

        public int LoadApiResourceFileIntoCache(ResourceTypes resourceType)
        {
            string resourceTypeName = Enum.GetName(typeof(ResourceTypes), resourceType).ToLower();
            string absoluteResourceDir = $"{base.CACHE_DIRECTORY}\\{resourceTypeName}";
            string absoluteFilepath = $"{absoluteResourceDir}{base.FILE_EXTENSION}";

            if (!File.Exists(absoluteFilepath))
                return 0;

            string[] lines = File.ReadAllLines(absoluteFilepath);
            CachedResource resource;
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());

            foreach (string line in lines)
            {
                resource = JsonSerializer.Deserialize<CachedResource>(line, options);
                string fileKey = $"{ resourceTypeName }_{ resource.Id}";
                resource.Json = File.ReadAllText($"{absoluteResourceDir}/{fileKey}{base.FILE_EXTENSION}");
                this.Db.StringSet($"_{fileKey}", JsonSerializer.Serialize(resource));
            }

            return lines.Length;
        }

        public int GetKeyCount()
        {
            return this.GetKeys("*").Count();
        }

        public async Task<IEnumerable<NamedCachedResource>> GetCachedNamedResourcesByPatternAsync(string pattern)
        {
            IEnumerable<RedisKey> keys = this.GetKeys(pattern);
            IList<NamedCachedResource> results = new List<NamedCachedResource>();

            foreach (RedisKey key in keys)
            {
                string keyString = key.ToString();

                if (key.ToString() == "(null)")
                    continue;

                results.Add(await this.GetCachedNamedResourceAsync(keyString));
            }

            return results;
        }

        public async Task<NamedCachedResource> GetCachedNamedResourceAsync(string key)
        {
            string result = await this.Db.StringGetAsync(key);

            if (string.IsNullOrEmpty(result))
                return null;

            return JsonSerializer.Deserialize<NamedCachedResource>(result);
        }

        public bool UpdateNamedCachedResource(NamedCachedResource cachedResource)
        {
            return this.Db.StringSet(cachedResource.Name, JsonSerializer.Serialize(cachedResource));
        }

        public async Task<IEnumerable<CachedResource>> GetCachedResourcesByPatternAsync(string pattern)
        {
            IEnumerable<RedisKey> keys = this.GetKeys(pattern);
            IList<CachedResource> results = new List<CachedResource>();

            foreach (RedisKey key in keys)
            {
                string keyString = key.ToString();

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
            string resourceTypeName = Enum.GetName(typeof(ResourceTypes), cachedResource.ResourceType).ToLower();
            return this.Db.StringSet($"_{resourceTypeName}_{cachedResource.Id}", JsonSerializer.Serialize(cachedResource));
        }

        private IEnumerable<RedisKey> GetKeys(string pattern)
        {
            IServer server = this.Redis.GetServer(this.Redis.GetEndPoints().First());
            return server.Keys(pattern: pattern);
        }
    }
}
