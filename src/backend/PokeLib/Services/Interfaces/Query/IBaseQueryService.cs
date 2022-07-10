using System.Collections.Generic;
using System.Threading.Tasks;
using PokeLib.Models;

namespace PokeLib.Services
{
    public interface IBaseQueryService
    {
        Task<IEnumerable<CachedResource>> QueryAsync(QueryOptions json);
        Task<string> QueryJsonAsync(QueryOptions json);
    }
}
