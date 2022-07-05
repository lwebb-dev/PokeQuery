using System;

namespace PokeLib.Cache
{
    public interface IBaseCache
    {
        Guid InstanceId { get; }
        void LoadResourceFileIntoCache(string fileDirectory);
    }
}
