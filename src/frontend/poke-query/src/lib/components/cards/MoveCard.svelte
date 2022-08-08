<script>
  import TypeModal from "../modals/TypeModal.svelte";

  export let data;

  console.log("Hello from MoveCard.svelte!");

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
      {data.name.replaceAll("-", " ")}
    </h4>
    <div class="d-flex justify-content-center">
      <TypeModal pkmnType={data.type} />
    </div>
    <div class="d-flex flex-wrap justify-content-center my-2" style="column-gap: 1rem;">
      <p class="move-attribute"><strong>Power:</strong> {handleNull(data.power)}</p>
      <p class="move-attribute"><strong>Accuracy:</strong> {handleNull(data.accuracy)}</p>
      <p class="move-attribute"><strong>PP:</strong> {handleNull(data.pp)}</p>
      <p class="move-attribute text-capitalize"><strong>Type:</strong> {handleNull(data.damage_class.name)}</p>
    </div>
    <div class="d-flex flex-wrap justify-content-center">
      {#if typeof data.effect_entries[0] !== "undefined"}
        <p class="card-text text-wrap">
          {interpolateEffectChance(
            data.effect_entries[0].short_effect,
            data.effect_chance
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
