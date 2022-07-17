<script>
    import { typeDataStore } from "../../../store.js";
    import TypeChiclet from "../TypeChiclet.svelte";

  export let pkmnType;

  let typeDataValue;

  typeDataStore.subscribe(value => {
    typeDataValue = value;
});
  
  let typeName = pkmnType.Name;
  let modalName = `typeModal-${typeName}`;
  // @ts-ignore
  let typeData = JSON.parse(typeDataValue.replaceAll(';', ''));
  let damageRelations = typeData.find(x => x.name === typeName).damageRelations;
</script>

<!-- Button trigger modal -->
    <TypeChiclet typeName={typeName} isStatic={false} />
  
      <!-- Modal -->
  <div class="modal fade" id="{modalName}" tabindex="-1" aria-labelledby="{modalName}-Label" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title text-capitalize" id="{modalName}-Label">Type: {typeName}</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        <div class="modal-body">
          <div class="container px-5">
            <div class="container title-border">
                <h4>Offensive</h4>
                {#if damageRelations.doubleDamageTo.length > 0}
                <h5>Strong Against:</h5>
                <div class="d-flex flex-wrap justify-content-center">
                    {#each damageRelations.doubleDamageTo as type}
                        <TypeChiclet typeName={type.name} isStatic={true} />
                    {/each}
                </div>
                {/if}
                {#if damageRelations.halfDamageTo.length > 0}
                <h5>Weak Against:</h5>
                <div class="d-flex flex-wrap justify-content-center">
                    {#each damageRelations.halfDamageTo as type}
                        <TypeChiclet typeName={type.name} isStatic={true} />
                    {/each}
                </div>
                {/if}
                {#if damageRelations.noDamageTo.length > 0}
                <h5>Resisted By:</h5>
                <div class="d-flex flex-wrap justify-content-center">
                    {#each damageRelations.noDamageTo as type}
                        <TypeChiclet typeName={type.name} isStatic={true} />
                    {/each}
                </div>
                {/if}
            </div>
            <div class="container title-border">
                <h4>Defensive</h4>
                {#if damageRelations.halfDamageFrom.length > 0}
                <h5>Strong Against:</h5>
                <div class="d-flex flex-wrap justify-content-center">
                    {#each damageRelations.halfDamageFrom as type}
                        <TypeChiclet typeName={type.name} isStatic={true} />
                    {/each}
                </div>
                {/if}
                {#if damageRelations.doubleDamageFrom.length > 0}
                <h5>Weak Against:</h5>
                <div class="d-flex flex-wrap justify-content-center">
                    {#each damageRelations.doubleDamageFrom as type}
                        <TypeChiclet typeName={type.name} isStatic={true} />
                    {/each}
                </div>
                {/if}
                {#if damageRelations.noDamageFrom.length > 0}
                <h5>Immune To:</h5>
                <div class="d-flex flex-wrap justify-content-center">
                    {#each damageRelations.noDamageFrom as type}
                        <TypeChiclet typeName={type.name} isStatic={true} />
                    {/each}
                </div>
                {/if}
            </div>

          </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-primary" data-bs-dismiss="modal">Close</button>
        </div>
      </div>
    </div>
  </div>

  <style>
    .title-border {
        border: 1px solid black;
        margin-bottom: 1rem;
        padding: 1rem;
    }

    .title-border h4 {
        margin-top: -2rem;
        margin-left: 10px;
        padding-left: 1rem;
        background-color: white;
        display: block;
        width: 130px;
    }
  </style>