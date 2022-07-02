using PokeLib;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeQuery.Services
{
    public interface IQueryService
    {
        Task<IEnumerable<CachedResource>> QueryAsync(string query);
        Task<string> QueryJsonAsync(string query);
    }
}
