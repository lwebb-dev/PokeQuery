<script>
  import MovesModal from "../modals/MovesModal.svelte";
  import StatsModal from "../modals/StatsModal.svelte";
  import TypeModal from "../modals/TypeModal.svelte";

  export let data;

  console.log("Hello from PokemonCard.svelte!");

</script>

<div class="card mw-20 m-2 card-pkmn">
  <img
    class="card-img-top h-45 pt-2 ps-5 pe-5"
    src={data.sprites.other.home.front_default.replace("https://raw.githubusercontent.com/PokeAPI/sprites/master/https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites", "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites")}
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
    height: 20rem;
  }
</style>
