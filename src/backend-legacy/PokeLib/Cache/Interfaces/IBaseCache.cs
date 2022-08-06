using System;

namespace PokeLib.Cache
{
    public interface IBaseCache
    {
        Guid InstanceId { get; }
        int LoadNamedResourceFileIntoCache(NamedResourceTypes namedResourceType);
    }
}
