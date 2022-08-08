using StackExchange.Redis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeQuery.Services
{
    public interface IRedisService
    {
        Task<IEnumerable<string>> QueryIndexJsonAsync(string index, string query);
        Task<IEnumerable<string>> GetJsonResultsByPatternAsync(string pattern);
    }
}
