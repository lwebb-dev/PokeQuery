using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeLib.Services
{
    public interface IBaseQueryService
    {
        Task<IEnumerable<CachedResource>> QueryAsync(string query);
        Task<string> QueryJsonAsync(string query);
    }
}
