using System;

namespace PokeLib.Cache
{
    public abstract class BaseCache : IBaseCache
    {
        internal string FILE_EXTENSION => ".txt";
        internal string[] RESOURCE_TYPES => new[] { "pokemon", "moves", "items"};

        internal readonly string CACHE_DIRECTORY;
        public Guid InstanceId { get; }

        public BaseCache(string cacheDirectory)
        {
            this.CACHE_DIRECTORY = cacheDirectory;
            this.InstanceId = Guid.NewGuid();
        }

        public abstract void LoadResourceFileIntoCache(string fileDirectory);

    }
}
