using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("/{prefix}")]
        public async Task<ActionResult<string>> GetAllJsonResultAsync(string prefix)
        {
            // var result = await this.queryService.GetJsonResultsByPatternAsync($"{prefix}:*")
            var result = await this.queryService.GetJsonResultsByPatternAsync($"{prefix}:*");
            return Ok(result);
        }


        [HttpGet("/{prefix}/{id}")]
        public async Task<ActionResult<string>> GetJsonResultByIdAsync(string prefix, int id)
        {
            return Ok(await this.queryService.GetJsonResultAsync($"{prefix}:{id}"));
        }

        [HttpGet("/search/{prefix}/{query}")]
        public async Task<ActionResult<IEnumerable<string>>> QueryIndexJsonAsync(string prefix, string query)
        {
            // Redis idx Pattern
            // var result = await this.queryService.QueryIndexJsonAsync($"idx:{prefix}", query);
            var result = await this.queryService.QueryIndexJsonAsync(prefix, query);
            return Ok(result);
        }
    }
}