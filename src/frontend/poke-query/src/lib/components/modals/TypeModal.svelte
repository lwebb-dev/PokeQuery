<script>
    import TypeChiclet from "../TypeChiclet.svelte";

  export let pkmnType;
  
  let typeName = pkmnType.Name;
  let modalName = `typeModal-${typeName}`;
</script>

<!-- Button trigger modal -->
    <TypeChiclet typeName={typeName} isStatic="false" />
  
      <!-- Modal -->
  <div class="modal fade" id="{modalName}" tabindex="-1" aria-labelledby="{modalName}-Label" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title text-capitalize" id="{modalName}-Label">Type: {typeName}</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        <div class="modal-body">
          <div class="container px-5">
            <TypeChiclet typeName={typeName} isStatic="true" />
          </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-primary" data-bs-dismiss="modal">Close</button>
        </div>
      </div>
    </div>
  </div>