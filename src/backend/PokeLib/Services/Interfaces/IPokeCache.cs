using System;
using System.Collections.Generic;

namespace PokeLib.Services
{
    public interface IPokeCache
    {
        IList<CachedResource> Cache { get; }
        Guid InstanceId { get; }
    }
}
