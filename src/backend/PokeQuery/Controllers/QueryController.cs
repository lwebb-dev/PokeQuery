using Microsoft.AspNetCore.Mvc;
using PokeLib;
using PokeQuery.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeQuery.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueryController : ControllerBase
    {
        private readonly IQueryService queryService;

        public QueryController(IQueryService queryService)
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