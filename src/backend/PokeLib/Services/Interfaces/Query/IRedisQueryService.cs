using PokeApiNet;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeLib.Services
{
    public interface IRedisQueryService : IBaseQueryService
    {
        Task<IEnumerable<T>> GetNamedResourceAsync<T>(NamedResourceTypes namedResourceType) where T : NamedApiResource;
        Task<IEnumerable<T>> GetApiResourceAsync<T>(ResourceTypes resourceType) where T : ApiResource;
    }
}
