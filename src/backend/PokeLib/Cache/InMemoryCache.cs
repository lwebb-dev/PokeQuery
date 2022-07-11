using System;
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

            foreach (ResourceTypes resourceType in Enum.GetValues(typeof(ResourceTypes)))
            {
                this.LoadResourceFileIntoCache(resourceType);
            }
        }

        /// <summary>
        /// Loads a text file from local filesystem into Cache object.
        /// </summary>
        /// <param name="fileDirectory">Exact file location on local filesystem</param>
        /// <returns>Number of new objects added to Cache</returns>
        public override int LoadResourceFileIntoCache(ResourceTypes resourceType)
        {
            string resourceTypeName = Enum.GetName(typeof(ResourceTypes), resourceType).ToLower();
            string fileDirectory = $"{base.CACHE_DIRECTORY}\\{resourceTypeName}{base.FILE_EXTENSION}";
            string[] lines;

            if (!File.Exists(fileDirectory))
                return 0;

            lines = File.ReadAllLines(fileDirectory);

            foreach (string line in lines)
            {
                this.Cache.Add(JsonSerializer.Deserialize<CachedResource>(line));
            }

            return lines.Length;
        }
    }
}
