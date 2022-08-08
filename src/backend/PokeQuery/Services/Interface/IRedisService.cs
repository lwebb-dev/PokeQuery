using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeQuery.Services
{
    public interface IRedisService
    {
        Task<IEnumerable<string>> GetJsonResultsByPatternAsync(string pattern);
    }
}
