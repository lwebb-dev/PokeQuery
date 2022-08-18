<script>
  export let data;
</script>

<div class="card mw-20 m-2 card-item">
  <img
    class="card-img-top h-35  pt-2 ps-5 pe-5"
    src={data.sprites.default.replace("https://raw.githubusercontent.com/PokeAPI/sprites/master/https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites", "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites")}
    alt={data.name}
  />
  <div class="card-body">
    <h4 class="card-title text-capitalize text-center">
      {data.name.replaceAll("-", " ")}
    </h4>
    {#if typeof data.effect_entries[0] !== "undefined"}
      <p class="card-text text-wrap">
        {data.effect_entries[0].short_effect}
      </p>
    {/if}
  </div>
</div>

<style>

  .h-35 {
    height: 35%;
  }

  .card-item {
    border-color: #ecf296;
    border-width: 0.35em;
    font-size: 0.85rem;
    width: 15rem;
    height: 20rem;
  }
</style>
