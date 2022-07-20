using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeLib.Cache
{
    public interface IRedisCache : IBaseCache
    {
        Task<IEnumerable<NamedCachedResource>> GetCachedResourcesByPatternAsync(string pattern);
        Task<NamedCachedResource> GetCachedResourceAsync(string key);
        bool UpdateCachedResource(NamedCachedResource cachedResource);
        int GetKeyCount();
    }
}
