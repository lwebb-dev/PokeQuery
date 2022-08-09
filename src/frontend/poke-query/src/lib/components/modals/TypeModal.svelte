<script>
    import TypeChiclet from "../TypeChiclet.svelte";

  export let pkmnType;
  
  let typeName = pkmnType.name;
  let modalName = `typeModal-${typeName}`;
  let typeData = JSON.parse(sessionStorage.typeData);

    for (let i = 0; i < typeData.length; i++) {
      typeData[i] = JSON.parse(typeData[i]);
    }

  let damageRelations = typeData.find(x => x.name === typeName).damage_relations;
</script>

<!-- Button trigger modal -->
    <TypeChiclet typeName={typeName} isStatic={false} />
  
      <!-- Modal -->
  <div class="modal fade" id="{modalName}" tabindex="-1" aria-labelledby="{modalName}-Label" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable">
      <div class="modal-content">
        <div class="modal-header">
          <h6 class="modal-title text-capitalize" id="{modalName}-Label">Type: {typeName}</h6>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        <div class="modal-body">
          <div class="container px-5">
            <div class="container title-border">
                <h4>Offensive</h4>
                {#if damageRelations.double_damage_to.length > 0}
                <h6>Strong Against:</h6>
                <div class="d-flex flex-wrap justify-content-center">
                    {#each damageRelations.double_damage_to as type}
                        <TypeChiclet typeName={type.name} isStatic={true} />
                    {/each}
                </div>
                {/if}
                {#if damageRelations.half_damage_to.length > 0}
                <h6>Weak Against:</h6>
                <div class="d-flex flex-wrap justify-content-center">
                    {#each damageRelations.half_damage_to as type}
                        <TypeChiclet typeName={type.name} isStatic={true} />
                    {/each}
                </div>
                {/if}
                {#if damageRelations.no_damage_to.length > 0}
                <h6>Resisted By:</h6>
                <div class="d-flex flex-wrap justify-content-center">
                    {#each damageRelations.no_damage_to as type}
                        <TypeChiclet typeName={type.name} isStatic={true} />
                    {/each}
                </div>
                {/if}
            </div>
            <div class="container title-border">
                <h4>Defensive</h4>
                {#if damageRelations.half_damage_from.length > 0}
                <h6>Strong Against:</h6>
                <div class="d-flex flex-wrap justify-content-center">
                    {#each damageRelations.half_damage_from as type}
                        <TypeChiclet typeName={type.name} isStatic={true} />
                    {/each}
                </div>
                {/if}
                {#if damageRelations.double_damage_from.length > 0}
                <h6>Weak Against:</h6>
                <div class="d-flex flex-wrap justify-content-center">
                    {#each damageRelations.double_damage_from as type}
                        <TypeChiclet typeName={type.name} isStatic={true} />
                    {/each}
                </div>
                {/if}
                {#if damageRelations.no_damage_from.length > 0}
                <h6>Immune To:</h6>
                <div class="d-flex flex-wrap justify-content-center">
                    {#each damageRelations.no_damage_from as type}
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