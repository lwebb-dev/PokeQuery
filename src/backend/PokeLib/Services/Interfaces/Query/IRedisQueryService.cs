using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeLib.Services
{
    public interface IRedisQueryService : IBaseQueryService
    {
        Task<IEnumerable<PokeApiNet.Type>> GetTypesAsync();
    }
}
