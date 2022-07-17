<script>
  import MovesModal from "../modals/MovesModal.svelte";
import StatsModal from "../modals/StatsModal.svelte";
  import TypeModal from "../modals/TypeModal.svelte";

  export let data;
</script>

<div class="card mw-20 m-2 card-pkmn">
  <img
    class="card-img-top h-35 pt-2 ps-5 pe-5"
    src={data.json.Sprites.Other.Home.FrontDefault}
    alt={data.name}
  />
  <div class="card-body">
    <h4 class="card-title text-capitalize text-center">
      {data.json.Name.replaceAll("-", " ")}
    </h4>
    <div class="d-flex justify-content-center">
      {#each data.json.Types as pkmnType}
        <TypeModal pkmnType={pkmnType.Type} />
      {/each}
    </div>
    <div class="d-flex mt-1 flex-wrap justify-content-center">
      <StatsModal data={data} />
      <MovesModal data={data} />
    </div>
  </div>
</div>

<style>
  .h-35 {
    height: 35%;
  }

  .card-pkmn {
    border-color: #fc8686;
    border-width: 0.35em;
    width: 15rem;
    height: 20rem;
  }
</style>
