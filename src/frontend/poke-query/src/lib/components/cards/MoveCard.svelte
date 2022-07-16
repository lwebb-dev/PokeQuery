<script>
import TypeChiclet from "../TypeChiclet.svelte";


  export let data;

  const handleNull = (value) => {
    if (value === null || value < 5)
      return "--"

    return value;
  }

  const interpolateEffectChance = (effect, chance) => {
    return effect.replaceAll("$effect_chance%", `${chance}%`);
  };
</script>

<div class="card mw-20 m-2 card-move">
  <div class="card-body">
    <h4 class="card-title text-capitalize text-center">
      {data.json.Name.replaceAll("-", " ")}
    </h4>
    <div class="d-flex justify-content-center" style="height:3em;">
      <TypeChiclet typeName={data.json.Type.Name} />
    </div>
    <div class="d-flex flex-wrap justify-content-center my-2" style="column-gap: 1rem;">
      <p class="move-attribute"><strong>Power:</strong> {handleNull(data.json.Power)}</p>
      <p class="move-attribute"><strong>Accuracy:</strong> {handleNull(data.json.Accuracy)}</p>
      <p class="move-attribute"><strong>PP:</strong> {handleNull(data.json.Pp)}</p>
      <p class="move-attribute text-capitalize"><strong>Type:</strong> {handleNull(data.json.DamageClass.Name)}</p>
    </div>
    <div class="d-flex flex-wrap justify-content-center">
      {#if typeof data.json.EffectEntries[0] !== "undefined"}
        <p class="card-text text-wrap">
          {interpolateEffectChance(
            data.json.EffectEntries[0].ShortEffect,
            data.json.EffectChance
          )}
      </p>
      {/if}
    </div>
  </div>
</div>

<style>
  .card-move {
    border-color: #bee4fa;
    border-width: 0.35em;
    width: 15rem;
    height: 20rem;
  }

  .move-attribute {
    margin: 0;
  }
</style>
