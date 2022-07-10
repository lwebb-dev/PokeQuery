using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeLib.Cache
{
    public interface IRedisCache : IBaseCache
    {
        Task<IEnumerable<CachedResource>> GetCachedResourcesByPatternAsync(string pattern);
        Task<CachedResource> GetCachedResourceAsync(string key);
        bool UpdateCachedResource(CachedResource cachedResource);
        int GetKeyCount();
    }
}
