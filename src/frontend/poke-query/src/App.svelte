<script lang="ts">

  let pokemon: any[] = [
    { name: "bulbasaur", type: "grass" },
    { name: "charmander", type: "fire" },
    { name: "squirtle", type: "water" },
    { name: "pikachu", type: "electric" }
  ];
  let results: any[] = [];
  let resultIndex: number;
  let query: string;

  function handleKeyDown(event) {
    if (event.keyCode === 13) {
      handleSearch();
    }
  }

  function handleSearch(): any {
    results = [];

    if (query === null || typeof(query) === "undefined" || query === "") {
      return;
    }

    while (pokemon.findIndex((x) => x.name.search(query) > -1 ) !== -1) {
      resultIndex = pokemon.findIndex((x) => x.name.search(query) > -1 );
      results.push(pokemon[resultIndex]);
      pokemon.splice(resultIndex, 1);
    }

    console.log(results);
    pokemon = [
      { name: "bulbasaur", type: "grass" },
      { name: "charmander", type: "fire" },
      { name: "squirtle", type: "water" },
      { name: "pikachu", type: "electric" }
    ];
  }

</script>

<main>
  <h1>PokeQuery</h1>

  <input type="text" bind:value={query} minlength="1" />
  <button on:click={handleSearch}>Search 🔎</button>
  <div class="cbSection">
    <div class="checkbox">
      <input id ="cbPkmn" type="checkbox" checked />
      <label for="cbPkmn">Pokemon</label>  
    </div>
    <div class="checkbox">
      <input id ="cbItems" type="checkbox" />
      <label for="cbItems">Items</label>  
    </div>
    <div class="checkbox">
      <input id ="cbMoves" type="checkbox" />
      <label for="cbMoves">Moves</label>  
    </div>  
  </div>

  <div class="resultContainer">
    {#each results as result}
    <div class="resultItem">
      <h3>{result.name}</h3>
      <h4>{result.type}</h4>
    </div>
    {/each}
  </div>

</main>

<svelte:window on:keydown={handleKeyDown} />

<style>

  :root {
    background-color: #595959;
    color: #EAEAEA;
  }

  main {
    text-align: center;
    padding: 1em;
    margin: 0 auto;
  }

  /* input[type=text] {
    border: none;
    background-color: #EAEAEA;
    height: 2em;
    
  } */

  .cbSection {
    display: flex;
    justify-content: center;
  }

  .resultContainer {
    display: flex;
    justify-content: center;

  }

  .resultItem {
      background-color: #EAEAEA;
      text-align: center;
      color: black;
      padding: 1em 1em 1em 0em;
      margin: 1em;
      border-radius: 15%; 
      height: 8em;
      width: 8em;
      box-shadow: 10px 10px 5px rgba(0, 0, 0, 0.2);
    }

</style>
