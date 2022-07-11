<script lang="ts">

  enum ResourceTypes {
    Pokemon = 0,
    Moves = 1, 
    Items = 2
  }

  let results: any[] = [];
  let query: string;
  let includePokemon: boolean = true;
  let includeItems: boolean = false;
  let includeMoves: boolean = false;

  const interpolateEffectChance = (effect: string, chance: number): string => {
    return effect.replaceAll("$effect_chance%", `${chance}%`);
  };

  const handleKeyDown = (event): void => {
    if (event.keyCode === 13) {
      handleSearch();

    }
  }

  const handleSearch = async () => {
    results = [];

    if (query === null || typeof(query) === "undefined" || query === "") {
      return;
    }

    let requestUri: string = `${process.env.API_BASE_URI}/query`;
    let requestBody = {
      Query: query, 
      IncludePokemon: includePokemon,
      IncludeItems: includeItems,
      IncludeMoves: includeMoves
    };

    await fetch(requestUri, {
    method: 'POST',
    headers: {
      'Accept': 'application/json',
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(requestBody)
  })
    .then(r => {
      if (!r.ok) {
        console.log("API FAILED TO RETURN 200 OK");
        return;
      }
      return r.json();
    })
    .then(data => {
      results = data;
      results.forEach(x => x.json = JSON.parse(x.json));
  });
  console.log(results)
}

</script>

<div class="container-fluid">

    <h1 class="row mb-3 justify-content-center">PokeQuery</h1>

    <div class="row justify-content-center">
      <div class="col col-lg-3">
        <input type="text" class="form-control form-control-lg" placeholder="pikachu, leftovers, etc..." bind:value={query} minlength="1" />
      </div>
      <div class="col-auto">
        <button type="submit" class="btn btn-primary btn-lg mb-3" on:click={handleSearch}>Search 🔎</button>
      </div>
      <div class="form-check d-flex justify-content-center">
        <div class="form-check form-check-inline">
          <input class="form-check-input" id="cbPkmn" type="checkbox" bind:checked={includePokemon} />
          <label class="form-check-label" for="cbPkmn">Pokemon</label>  
        </div>
        <div class="form-check form-check-inline">
          <input class="form-check-input" id ="cbItems" type="checkbox" bind:checked={includeItems} />
          <label class="form-check-label" for="cbItems">Items</label>  
        </div>
        <div class="form-check form-check-inline">
          <input class="form-check-input" id ="cbMoves" type="checkbox" bind:checked={includeMoves} />
          <label class="form-check-label" for="cbMoves">Moves</label>  
        </div>  
      </div>
    </div>  

  <div class="container d-flex flex-wrap justify-content-center">

    {#each results as result}

      {#if result.resourceType == ResourceTypes.Pokemon}
        <div class="card mw-20 m-2 card-pkmn">
          <img class="card-img-top h-25 pt-2 ps-5 pe-5" src="{result.json.Sprites.FrontDefault}" alt="{result.name}"/>
          <div class="card-body">
            <h4 class="card-title text-capitalize text-center">{result.json.Name}</h4>
            <div class="d-flex justify-content-center">
              {#each result.json.Types as pkmnType}
                <div class="card-text m-2 text-capitalize">
                  <p>{pkmnType.Type.Name}</p>
                </div>
              {/each}
            </div>
            <p class="card-text">Here's some more filler text to give the test cqrds some more body untill we parse the pokemon json data and add more of it to the display.</p>  
          </div>
        </div>

      {:else if result.resourceType == ResourceTypes.Items}
      <div class="card mw-20 m-2 card-item">
        <img  class="card-img-top h-25 pt-2 ps-5 pe-5" src="{result.json.Sprites.Default}" alt="{result.name}"/>
        <div class="card-body">
          <h4 class="card-title text-capitalize text-center">{result.json.Name}</h4>
          {#if typeof(result.json.EffectEntries[0]) !== "undefined"}
            <p class="card-text text-wrap">{result.json.EffectEntries[0].ShortEffect}</p>
          {/if}  
        </div>
      </div>

      {:else}
        <div class="card mw-20 m-2 card-move">
          <div class="card-body">
            <h4 class="card-title text-capitalize text-center">{result.json.Name}</h4>
            {#if typeof(result.json.EffectEntries[0]) !== "undefined"}
              <p class="card-text text-wrap">{interpolateEffectChance(result.json.EffectEntries[0].ShortEffect, result.json.EffectChance)}</p>
            {/if}  
          </div>
        </div>

      {/if}
    {/each}
  </div>

</div>

<svelte:window on:keydown={handleKeyDown} />

<style>

  .card-pkmn {
    background-color: #FC8686;
    width: 18rem;
  }

  .card-item {
    background-color: #ECF296;
    width: 18rem;
  }

  .card-move {
    background-color: #BEE4FA;
    width: 18rem;
  }


</style>
