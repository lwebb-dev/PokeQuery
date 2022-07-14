<script>
import TypeChiclet from "./TypeChiclet.svelte";

    export let data;
</script>

<div class="card mw-20 h-50 m-2 card-pkmn">
    <img class="card-img-top h-35 pt-2 ps-5 pe-5" src="{data.json.Sprites.Other.Home.FrontDefault}" alt="{data.name}"/>
    <div class="card-body">
      <h4 class="card-title text-capitalize text-center">{data.json.Name.replaceAll('-', ' ')}</h4>
      <div class="d-flex justify-content-center" style="height:3em;">
        {#each data.json.Types as pkmnType}
        <TypeChiclet typeName={pkmnType.Type.Name}/>
          <!-- <div class="card-text m-2 pt-1 ps-3 pe-3 text-capitalize" style="background-color:{typeColors[pkmnType.Type.Name]};border-radius:15px;color:{getFontColorByPkmnType(pkmnType.Type.Name)};font-size:small">
            <p>{pkmnType.Type.Name}</p>
          </div> -->
        {/each}
      </div>
      <!-- <p class="card-text">Here's some more filler text to give the test cards some more body untill we parse the pokemon json data and add more of it to the display.</p>   -->
    </div>
  </div>

<style>
    .h-35 {
        height: 35%;
    }
     
    .card-pkmn {
        border-color: #FC8686;
        border-width: 0.35em;
        width: 15rem;
    }
</style>