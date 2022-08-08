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

        [HttpGet("/types")]
        public async Task<ActionResult<IEnumerable<string>>> GetTypesJsonAsync()
        {
            return Ok(await this.redisService.GetJsonResultsByPatternAsync("type:*"));
        }

        [HttpGet("/version-groups")]
        public async Task<ActionResult<IEnumerable<string>>> GetVersionGroupsJsonAsync()
        {
            return Ok(await this.redisService.GetJsonResultsByPatternAsync("*version-group:*"));
        }

        [HttpGet("/generations")]
        public async Task<ActionResult<IEnumerable<string>>> GetGenerationsJsonAsync()
        {
            return Ok(await this.redisService.GetJsonResultsByPatternAsync("*generation:*"));
        }

        [HttpGet("/machines")]
        public async Task<ActionResult<IEnumerable<string>>> GetMachinesJsonAsync()
        {
            return Ok(await this.redisService.GetJsonResultsByPatternAsync("*machine:*"));
        }
    }
}