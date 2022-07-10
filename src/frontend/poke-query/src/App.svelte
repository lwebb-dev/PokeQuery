<script lang="ts">

  let results: any[] = [];
  let query: string;
  let includePokemon: boolean = true;
  let includeItems: boolean = false;
  let includeMoves: boolean = false;

  const capitalize = (value: string): string => {
    
    let words: string[] = value.split('-');
    let newWords: string[] = [];

    words.forEach((x) => {
      let chars = x.split('');
      chars[0] = x.toUpperCase()[0];
      newWords.push(chars.join(''));
    });

    return newWords.join(' ');
  }

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

    console.log(requestBody);

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
  console.log(results[0].json)
}

</script>

<main>
  <h1>PokeQuery</h1>

  <input type="text" bind:value={query} minlength="1" />
  <button on:click={handleSearch}>Search 🔎</button>
  <div class="cbSection">
    <div class="checkbox">
      <input id ="cbPkmn" type="checkbox" bind:checked={includePokemon} />
      <label for="cbPkmn">Pokemon</label>  
    </div>
    <div class="checkbox">
      <input id ="cbItems" type="checkbox" bind:checked={includeItems} />
      <label for="cbItems">Items</label>  
    </div>
    <div class="checkbox">
      <input id ="cbMoves" type="checkbox" bind:checked={includeMoves} />
      <label for="cbMoves">Moves</label>  
    </div>  
  </div>

  <div class="resultContainer">
    {#each results as result}
    <div class="resultItem">
      <!-- <img src="{result.json.Sprites.FrontDefault}" alt="{result.name}"/> -->
      <h3>{capitalize(result.json.Name)}</h3>
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
      padding: 1em 2em 1em 2em;
      margin: 1em;
      border-radius: 15%; 
      box-shadow: 10px 10px 5px rgba(0, 0, 0, 0.2);
    }

</style>
