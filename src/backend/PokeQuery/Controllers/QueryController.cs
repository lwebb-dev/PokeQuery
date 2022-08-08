using Microsoft.AspNetCore.Mvc;
using PokeApiNet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeQuery.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QueryController : ControllerBase
    {
        //private readonly IRedisQueryService queryService;

        public QueryController()
        {
            //this.queryService = queryService;
        }

        [HttpGet("/types")]
        public async Task<ActionResult<IEnumerable<PokeApiNet.Type>>> GetTypesAsync()
        {
            throw new NotImplementedException();
            //return Ok(await this.queryService.GetNamedResourceAsync<Type>(NamedResourceTypes.Types));
        }

        [HttpGet("/version-groups")]
        public async Task<ActionResult<IEnumerable<VersionGroup>>> GetVersionGroupsAsync()
        {
            throw new NotImplementedException();
            //return Ok(await this.queryService.GetNamedResourceAsync<VersionGroup>(NamedResourceTypes.VersionGroups));
        }

        [HttpGet("/generations")]
        public async Task<ActionResult<IEnumerable<Generation>>> GetGenerationsAsync()
        {
            throw new NotImplementedException();
            //return Ok(await this.queryService.GetNamedResourceAsync<Generation>(NamedResourceTypes.Generations));
        }

        [HttpGet("/machines")]
        public async Task<ActionResult<IEnumerable<Machine>>> GetMachinesAsync()
        {
            throw new NotImplementedException();
            //return Ok(await this.queryService.GetApiResourceAsync<Machine>(ResourceTypes.Machines));
        }
    }
}