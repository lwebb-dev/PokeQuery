<script>
    import { statNames } from '../../common';
    export let data;
    const up = "🢁";
    const down = "🢃";
</script>
  
  <div class="card mw-20 m-2 card-nature">
    <div class="card-body">
      <h2 class="mt-2 card-title text-capitalize text-center">
        {data.name}
      </h2>
      <div class="mt-5 nature-stats">
        {#if (data.increased_stat === null && data.decreased_stat === null)}
        <h3>Neutral</h3>
        {:else}
        <h3>{statNames[data.increased_stat.name]} <i class="bi bi-arrow-up-circle-fill" style="color: #ff0000;"></i></h3>
        <h3>{statNames[data.decreased_stat.name]} <i class="bi bi-arrow-down-circle-fill" style="color: #0000ff;"></i></h3>
        {/if}
      </div>
    </div>
  </div>
  
<style>
    .card-nature {
      border-color: #befad5;
      border-width: 0.35em;
      width: 15rem;
      height: 21rem;
    }

    .nature-stats {
        display: flex;
        flex-direction: column;
        align-items: center;
    }
</style>
  