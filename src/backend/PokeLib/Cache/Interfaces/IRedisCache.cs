using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeLib.Cache
{
    public interface IRedisCache : IBaseCache
    {
        int GetKeyCount();

        // Named Resources
        Task<IEnumerable<NamedCachedResource>> GetCachedNamedResourcesByPatternAsync(string pattern);
        Task<NamedCachedResource> GetCachedNamedResourceAsync(string key);
        bool UpdateNamedCachedResource(NamedCachedResource cachedResource);

        // Unnamed Api Resources
        Task<IEnumerable<CachedResource>> GetCachedResourcesByPatternAsync(string pattern);
        Task<CachedResource> GetCachedResourceAsync(string key);

        bool UpdateCachedResource(CachedResource cachedResource);
    }
}
