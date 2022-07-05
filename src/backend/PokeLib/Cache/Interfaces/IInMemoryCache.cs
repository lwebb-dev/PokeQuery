using System.Collections.Generic;

namespace PokeLib.Cache
{
    public interface IInMemoryCache : IBaseCache
    {
        IList<CachedResource> Cache { get; set; }
    }
}
