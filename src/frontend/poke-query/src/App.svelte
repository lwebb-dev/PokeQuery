<script lang="ts">
  import ItemCard from "./lib/components/cards/ItemCard.svelte";
  import MoveCard from "./lib/components/cards/MoveCard.svelte";
  import PokemonCard from "./lib/components/cards/PokemonCard.svelte";

  enum ResourceTypes {
    Pokemon = 0,
    Moves = 1,
    Items = 2,
  }

  let results: any[] = [];
  let query: string;
  let baseUri: string = process.env.API_BASE_URI;

  let loadingTypes: boolean = false;
  let isLoading: boolean = false;

  let includePokemon: boolean = true;
  let includeItems: boolean = false;
  let includeMoves: boolean = false;

  const init = async () => {
    if (typeof(sessionStorage.typeData) === "undefined") {
      loadingTypes = true;
      await fetch(`${baseUri}/types`, {
      method: "GET",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      }
    })
      .then((r) => {
        if (!r.ok) {
          loadingTypes = false;
          throw new Error("API FAILED TO RETURN 200 OK ON /TYPES");
        }
        return r.json();
      })
      .then((data) => {
        sessionStorage.typeData = JSON.stringify(data);
        loadingTypes = false;
      });

    }
  }

  const handleKeyDown = (event): void => {
    if (event.keyCode === 13) {
      handleSearch();
    }
  };

  const handleSearch = async () => {
    results = [];

    if (loadingTypes)
      return;

    if (query === null || typeof query === "undefined" || query === "") {
      return;
    }

    isLoading = true;

    let requestUri: string = `${baseUri}/query`;
    let requestBody = {
      Query: query,
      IncludePokemon: includePokemon,
      IncludeItems: includeItems,
      IncludeMoves: includeMoves,
    };

    await fetch(requestUri, {
      method: "POST",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      body: JSON.stringify(requestBody),
    })
      .then((r) => {
        if (!r.ok) {
          isLoading = false;
          throw new Error("API FAILED TO RETURN 200 OK");
        }
        return r.json();
      })
      .then((data) => {
        results = data;
        results.forEach((x) => (x.json = JSON.parse(x.json)));
      });

    isLoading = false;
    console.log(results);
  };

  init();
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
        {#if loadingTypes}
          <div class="spinner-border" role="status">
            <span class="visually-hidden">Loading...</span>
          </div>
        {:else}
          Search 🔎
        {/if}
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
    </div>
  </div>

  <div class="container mt-3 d-flex flex-wrap justify-content-center">
    {#if isLoading}
      <div class="spinner-border" role="status">
        <span class="visually-hidden">Loading...</span>
      </div>
    {/if}

    {#each results as result}
      {#if result.resourceType === ResourceTypes.Pokemon && result.json.IsDefault === true}
        <PokemonCard data={result} />
      {:else if result.resourceType === ResourceTypes.Items && !result.name.includes("-candy")}
        <ItemCard data={result} />
      {:else if result.resourceType === ResourceTypes.Moves}
        <MoveCard data={result} />
      {/if}
    {/each}
  </div>
</div>

<svelte:window on:keydown={handleKeyDown} />
