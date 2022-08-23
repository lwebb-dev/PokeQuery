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
        private readonly IRedisService redisService;

        public QueryController(IRedisService redisService)
        {
            this.redisService = redisService;
        }

        [HttpGet("/{prefix}")]
        public async Task<ActionResult<string>> GetAllJsonResultAsync(string prefix)
        {
            return Ok(await this.redisService.GetJsonResultsByPatternAsync($"{prefix}:*"));
        }


        [HttpGet("/{prefix}/{id}")]
        public async Task<ActionResult<string>> GetJsonResultByIdAsync(string prefix, int id)
        {
            return Ok(await this.redisService.GetJsonResultAsync($"{prefix}:{id}"));
        }

        [HttpGet("/search/{prefix}/{query}")]
        public async Task<ActionResult<IEnumerable<string>>> QueryIndexJsonAsync(string prefix, string query)
        {
            return Ok(await this.redisService.QueryIndexJsonAsync($"idx:{prefix}", query));
        }
    }
}