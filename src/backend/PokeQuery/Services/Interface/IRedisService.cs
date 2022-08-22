using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeQuery.Services
{
    public interface IRedisService
    {
        Task<IEnumerable<string>> QueryIndexJsonAsync(string index, string query);
        Task<IEnumerable<string>> GetJsonResultsByPatternAsync(string pattern);
        Task<string> GetJsonResultAsync(string key, string[] paths = null);
    }
}
