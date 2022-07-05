using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PokeLib.Cache
{
    public sealed class InMemoryCache : BaseCache, IInMemoryCache
    {
        public IList<CachedResource> Cache { get; set; }

        public InMemoryCache(string cacheDirectory) 
            : base(cacheDirectory)
        {
            this.Cache = new List<CachedResource>();

            foreach (string resourceType in base.RESOURCE_TYPES)
            {
                this.LoadResourceFileIntoCache($"{base.CACHE_DIRECTORY}\\{resourceType}{base.FILE_EXTENSION}");
            }
        }

        /// <summary>
        /// Loads a text file from local filesystem into Cache object.
        /// </summary>
        /// <param name="fileDirectory">Exact file location on local filesystem</param>
        /// <returns>Number of new objects added to Cache</returns>
        public override void LoadResourceFileIntoCache(string fileDirectory)
        {
            string[] lines;

            if (!File.Exists(fileDirectory))
                return;

            lines = File.ReadAllLines(fileDirectory);

            foreach (string line in lines)
            {
                this.Cache.Add(JsonSerializer.Deserialize<CachedResource>(line));
            }

            return;
        }
    }
}
