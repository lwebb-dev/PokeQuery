<script>
    export let data;

    const interpolateEffectChance = (effect, chance) => {
    return effect.replaceAll("$effect_chance%", `${chance}%`);
  };

</script>

<div class="card mw-20 h-50 m-2 card-move">
    <div class="card-body">
      <h4 class="card-title text-capitalize text-center">{data.json.Name.replaceAll('-', ' ')}</h4>
      {#if typeof(data.json.EffectEntries[0]) !== "undefined"}
        <p class="card-text text-wrap">{interpolateEffectChance(data.json.EffectEntries[0].ShortEffect, data.json.EffectChance)}</p>
      {/if}  
    </div>
  </div>

  <style>
    .card-move {
      border-color: #BEE4FA;
      border-width: 0.35em;
      width: 15rem;
    }
  </style>
  