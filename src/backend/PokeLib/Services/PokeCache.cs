using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PokeLib.Services
{
    public sealed class PokeCache : IPokeCache
    {
        public IList<CachedResource> Cache { get; }
        public Guid InstanceId { get; }

        public PokeCache()
        {
            this.Cache = new List<CachedResource>();
            this.InstanceId = Guid.NewGuid();
        }

        /// <summary>
        /// Loads a text file from local filesystem into Cache object.
        /// </summary>
        /// <param name="fileDirectory">Exact file location on local filesystem</param>
        /// <returns>Number of new objects added to Cache</returns>
        public int LoadResourceFileIntoCache(string fileDirectory)
        {
            int result = 0;
            string[] lines;

            if (!File.Exists(fileDirectory))
                return result;

            lines = File.ReadAllLines(fileDirectory);

            foreach (string line in lines)
            {
                this.Cache.Add(JsonSerializer.Deserialize<CachedResource>(line));
                result++;
            }

            return result;
        }

    }
}
