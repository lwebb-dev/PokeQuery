using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PokeLib.Cache
{
    public sealed class InMemoryCache : BaseCache, IInMemoryCache
    {
        public IList<NamedCachedResource> Cache { get; set; }

        public InMemoryCache(string cacheDirectory) 
            : base(cacheDirectory)
        {
            this.Cache = new List<NamedCachedResource>();

            foreach (NamedResourceTypes namedResourceType in Enum.GetValues(typeof(NamedResourceTypes)))
            {
                this.LoadNamedResourceFileIntoCache(namedResourceType);
            }
        }

        /// <summary>
        /// Loads a text file from local filesystem into Cache object.
        /// </summary>
        /// <param name="fileDirectory">Exact file location on local filesystem</param>
        /// <returns>Number of new objects added to Cache</returns>
        public override int LoadNamedResourceFileIntoCache(NamedResourceTypes namedResourceType)
        {
            string resourceTypeName = Enum.GetName(typeof(ResourceTypes), namedResourceType).ToLower();
            string fileDirectory = $"{base.CACHE_DIRECTORY}\\{resourceTypeName}{base.FILE_EXTENSION}";
            string[] lines;

            if (!File.Exists(fileDirectory))
                return 0;

            lines = File.ReadAllLines(fileDirectory);

            foreach (string line in lines)
            {
                this.Cache.Add(JsonSerializer.Deserialize<NamedCachedResource>(line));
            }

            return lines.Length;
        }
    }
}
