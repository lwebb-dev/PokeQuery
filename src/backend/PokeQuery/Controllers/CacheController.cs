using Microsoft.AspNetCore.Mvc;
using PokeQuery.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokeQuery.Controllers;

[ApiController]
[Route("[controller]")]
public class CacheController : ControllerBase
{
    private readonly IQueryService queryService;

    public CacheController(IQueryService queryService)
    {
        this.queryService = queryService;
    }

    [HttpGet("/pokemon/names")]
    public async Task<ActionResult<string>> CachePokemonNames()
    {
        var pokemonNames = await queryService.GetPokemonNamesAsync();
        Response.Headers.CacheControl = "public, max-age=86400";
        return Ok(pokemonNames);
    }
}