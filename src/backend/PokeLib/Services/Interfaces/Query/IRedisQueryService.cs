using PokeApiNet;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeLib.Services
{
    public interface IRedisQueryService : IBaseQueryService
    {
        Task<IEnumerable<T>> GetResourceAsync<T>(NamedResourceTypes namedResourceType) where T : NamedApiResource;
    }
}
