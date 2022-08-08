using Microsoft.AspNetCore.Mvc;
using PokeQuery.Services;
using StackExchange.Redis;
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

        [HttpGet("/pokemon")]
        public async Task<ActionResult<IEnumerable<string>>> SearchPokemonJsonAsync(string query)
        {
            return Ok(await this.redisService.QueryIndexJsonAsync("idx:pokemon", query));
        }

        [HttpGet("/items")]
        public async Task<ActionResult<IEnumerable<string>>> SearchItemJsonAsync(string query)
        {
            return Ok(await this.redisService.QueryIndexJsonAsync("idx:item", query));
        }

        [HttpGet("/moves")]
        public async Task<ActionResult<IEnumerable<string>>> SearchMoveJsonAsync(string query)
        {
            return Ok(await this.redisService.QueryIndexJsonAsync("idx:move", query));
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