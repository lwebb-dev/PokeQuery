using Microsoft.AspNetCore.Mvc;
using PokeLib;
using PokeLib.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeQuery.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueryController : ControllerBase
    {
        //private readonly IInMemoryCacheQueryService queryService;
        private readonly IRedisQueryService queryService;

        public QueryController(IRedisQueryService queryService)
        {
            this.queryService = queryService;
        }

        [HttpGet("/")]
        public async Task<IEnumerable<CachedResource>> GetAsync(string query)
        {
            return await this.queryService.QueryAsync(query);
        }

        [HttpGet("/json")]
        public async Task<string> GetJsonAsync(string query)
        {
            return await this.queryService.QueryJsonAsync(query);
        }
    }
}