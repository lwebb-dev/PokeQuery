using System.Threading.Tasks;

namespace PokeLib.Cache
{
    public interface IRedisCache : IBaseCache
    {
        Task<CachedResource> GetCachedResourceAsync(string key);
        bool UpdateCachedResource(CachedResource cachedResource);
        int GetKeyCount();
    }
}
