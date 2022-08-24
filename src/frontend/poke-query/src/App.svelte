<script lang="ts">
  import ItemCard from "./lib/components/cards/ItemCard.svelte";
  import MoveCard from "./lib/components/cards/MoveCard.svelte";
  import PokemonCard from "./lib/components/cards/PokemonCard.svelte";
  import NatureCard from "./lib/components/cards/NatureCard.svelte";
  import { isLoadingSessionData, loadSessionData } from "./lib/data/session";

  let pkmnResults: any[] = [];
  let itemResults: any[] = [];
  let moveResults: any[] = [];
  let natureResults: any[] = [];
  let query: string;
  let baseUri: string = process.env.API_BASE_URI;

  let isLoading: boolean = false;

  let includePokemon: boolean = true;
  let includeItems: boolean = false;
  let includeMoves: boolean = false;
  let includeNatures: boolean = false;

  const handleQuery = async (prefix: string, flag: boolean, results: any[]) => {

      if (!flag) {
        return Promise.resolve();
      }
  
      await fetch(`${baseUri}/search/${prefix}/${query}`, {
      method: "GET",
      headers: {
        "Accept": "application/json",
        "Content-Type": "application/json",
      },
    })
    .then((r) => {
      if (!r.ok) {
        throw new Error("API FAILED TO RETURN 200 OK ON /search");
      }
      return r.json();
    })
    .then((data) => {
      data.forEach(x => {
        let jsonObj = JSON.parse(x);
        results.push(jsonObj);
        results = results;
      })
    });
  };
    

  const handleSearch = async () => {
    if (isLoadingSessionData || query === null || typeof query === "undefined" || query === "") {
      return;
    }

    isLoading = true;
    
    return Promise.all([
      pkmnResults = [],
      itemResults = [],
      moveResults = [],
      natureResults = [],
      handleQuery("pokemon", includePokemon, pkmnResults),
      handleQuery("item", includeItems, itemResults),
      handleQuery("move", includeMoves, moveResults),
      handleQuery("nature", includeNatures, natureResults)
    ]).finally(() => {
      isLoading = false;
      pkmnResults = pkmnResults.sort((a,b) => a.id - b.id);
      console.log(`query: \"${query}\" | pkmnResults: ${pkmnResults.length} | itemResults: ${itemResults.length} | moveResults: ${moveResults.length} | natureResults: ${natureResults.length}`);
      console.log(pkmnResults);
      console.log(itemResults);
      console.log(moveResults);
      console.log(natureResults);
    });
  };

  const handleKeyDown = (event): void => {
    if (event.keyCode === 13) {
      handleSearch();
    }
  };

</script>

<div class="container-fluid my-3">
  <h1 class="row mb-3 justify-content-center">PokeQuery</h1>
  <div class="row justify-content-center">
    <div class="col col-lg-3">
      <input
        type="text"
        class="form-control form-control-lg"
        placeholder="pikachu, leftovers, etc..."
        bind:value={query}
        minlength="1"
      />
    </div>
    <div class="col-auto">
      <button
        type="submit"
        class="btn btn-primary btn-lg mb-3"
        on:click={handleSearch}>
        {#await loadSessionData(baseUri)}
          <div class="spinner-border" role="status">
            <span class="visually-hidden">Loading...</span>
          </div>
        {:then}
          Search 🔎
        {/await}
        </button
      >
    </div>
    <div class="form-check d-flex justify-content-center">
      <div class="form-check form-check-inline">
        <input
          class="form-check-input"
          id="cbPkmn"
          type="checkbox"
          bind:checked={includePokemon}
        />
        <label class="form-check-label" for="cbPkmn">Pokemon</label>
      </div>
      <div class="form-check form-check-inline">
        <input
          class="form-check-input"
          id="cbItems"
          type="checkbox"
          bind:checked={includeItems}
        />
        <label class="form-check-label" for="cbItems">Items</label>
      </div>
      <div class="form-check form-check-inline">
        <input
          class="form-check-input"
          id="cbMoves"
          type="checkbox"
          bind:checked={includeMoves}
        />
        <label class="form-check-label" for="cbMoves">Moves</label>
      </div>
      <div class="form-check form-check-inline">
        <input
          class="form-check-input"
          id="cbMoves"
          type="checkbox"
          bind:checked={includeNatures}
        />
        <label class="form-check-label" for="cbMoves">Natures</label>
      </div>
    </div>
  </div>

  <div class="container mt-3 d-flex flex-wrap justify-content-center">
    {#if isLoading}
      <div class="spinner-border" role="status">
        <span class="visually-hidden">Loading...</span>
      </div>
    {:else}
    {#each pkmnResults as pkmnResult}
      {#if pkmnResult.is_default === true}
        <PokemonCard data={pkmnResult} />
      {/if}
    {/each}

    {#each itemResults as itemResult}
    {#if !itemResult.name.includes("-candy")}
      <ItemCard data={itemResult} />
    {/if}
    {/each}

    {#each moveResults as moveResult}
      <MoveCard data={moveResult} />
    {/each}

    {#each natureResults as natureResult}
      <NatureCard data={natureResult} />
    {/each}
    {/if}
  </div>
</div>

<svelte:window on:keydown={handleKeyDown} />
