<script lang="ts">

  let results: any[] = [];
  let query: string;

  function handleKeyDown(event) {
    if (event.keyCode === 13) {
      handleSearch();

    }
  }

  async function handleSearch() {
    results = [];

    if (query === null || typeof(query) === "undefined" || query === "") {
      return;
    }

    let requestUri: string = `${process.env.API_BASE_URI}?query=${query}`;

    await fetch(requestUri)
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
  console.log(results[0])
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
      <h3>{result.json.Name}</h3>
      {#each result.json.Types as type}
        <h4>{type.Type.Name}</h4>
      {/each}
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
