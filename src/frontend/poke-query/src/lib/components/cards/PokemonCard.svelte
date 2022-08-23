<script>
  import MovesModal from "../modals/MovesModal.svelte";
  import StatsModal from "../modals/StatsModal.svelte";
  import TypeModal from "../modals/TypeModal.svelte";

  export let data;

  let shinyBtnSelected = false;

  const stripGarbageSpriteText = (text) => {
    return text?.replace(
      "https://raw.githubusercontent.com/PokeAPI/sprites/master/https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites", 
      "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites");
  }
</script>

<div class="card mw-20 m-2 card-pkmn">
  <button class="shiny-btn" class:shiny-selected="{shinyBtnSelected}" on:click="{() => shinyBtnSelected = !shinyBtnSelected}">✨</button>
  <img
    class="card-img-top h-45 pt-2 ps-5 pe-5"
    src={stripGarbageSpriteText(shinyBtnSelected ? data.sprites.other.home.front_shiny : data.sprites.other.home.front_default)}
    alt={data.name}
  />
  <div class="card-body">
    <h4 class="card-title text-capitalize text-center">
      {data.name.replaceAll("-", " ")}
    </h4>
    <div class="d-flex justify-content-center">
      {#each data.types as pkmnType}
        <TypeModal pkmnType={pkmnType.type} />
      {/each}
    </div>
    <div class="d-flex mt-1 flex-wrap justify-content-center">
      <StatsModal data={data} />
      <MovesModal data={data} />
    </div>
  </div>
</div>

<style>
  .h-45 {
    height: 45%;
  }

  .card-pkmn {
    border-color: #fc8686;
    border-width: 0.35em;
    width: 15rem;
    height: 21rem;
  }

  .shiny-btn {
    position: absolute;
    text-align: center;
    height: 40px;
    width: 40px;
    margin-top: 0.4rem;
    margin-left: 11rem;
    border-radius: 25%;
    border-color: rgba(0, 0, 0, 0.0);
    background-color: rgba(0, 0, 0, 0.0);
    filter: grayscale(100%);
    cursor: pointer;
    z-index: 5;
  }

  .shiny-btn:hover {
    background-color: #efef99;
  }

  .shiny-selected {
    filter: grayscale(100%) sepia(100%) saturate(500%);

  }
</style>
