<script>
  export let data;
</script>

<div class="card mw-20 h-50 m-2 card-item">
  <img
    class="card-img-top h-25  pt-2 ps-5 pe-5"
    src={data.json.Sprites.Default}
    alt={data.name}
  />
  <div class="card-body">
    <h4 class="card-title text-capitalize text-center">
      {data.json.Name.replaceAll("-", " ")}
    </h4>
    {#if typeof data.json.EffectEntries[0] !== "undefined"}
      <p class="card-text text-wrap">
        {data.json.EffectEntries[0].ShortEffect}
      </p>
    {/if}
  </div>
</div>

<style>
  .card-item {
    border-color: #ecf296;
    border-width: 0.35em;
    width: 15rem;
  }
</style>
