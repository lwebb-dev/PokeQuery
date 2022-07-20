using Microsoft.AspNetCore.Mvc;
using PokeApiNet;
using PokeLib;
using PokeLib.Models;
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

        [HttpPost("/query")]
        public async Task<ActionResult<IEnumerable<NamedCachedResource>>> GetAsync(QueryOptions json)
        {
            return Ok(await this.queryService.QueryAsync(json));
        }

        [HttpPost("/json")]
        public async Task<ActionResult<string>> GetJsonAsync(QueryOptions json)
        {
            return Ok(await this.queryService.QueryJsonAsync(json));
        }

        [HttpGet("/types")]
        public async Task<ActionResult<IEnumerable<Type>>> GetTypesAsync()
        {
            return Ok(await this.queryService.GetResourceAsync<Type>(NamedResourceTypes.Types));
        }

        [HttpGet("/version-groups")]
        public async Task<ActionResult<IEnumerable<VersionGroup>>> GetVersionGroupsAsync()
        {
            return Ok(await this.queryService.GetResourceAsync<VersionGroup>(NamedResourceTypes.VersionGroups));
        }

        [HttpGet("/generations")]
        public async Task<ActionResult<IEnumerable<Generation>>> GetGenerationsAsync()
        {
            return Ok(await this.queryService.GetResourceAsync<Generation>(NamedResourceTypes.Generations));
        }
    }
}