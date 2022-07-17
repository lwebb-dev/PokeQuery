<script>
    // @ts-nocheck
    
        export let data;
        const MAX_STAT_VALUE = 255;
        let modalName = `movesModal-${data.name}`;
        let moveData = data.json.Moves;
        let swshMoveData = moveData.filter(x => x.VersionGroupDetails.some((vgd) => vgd.VersionGroup.Name === "sword-shield"));
        swshMoveData.forEach(x => x.VersionGroupDetails = x.VersionGroupDetails.filter(vgd => vgd.VersionGroup.Name === "sword-shield"));
        let lvlUpMoveData = structuredClone(swshMoveData);
        lvlUpMoveData.forEach(x => x.VersionGroupDetails = x.VersionGroupDetails.filter(vgd => vgd.MoveLearnMethod.Name === "level-up"));
        lvlUpMoveData = lvlUpMoveData.filter(x => x.VersionGroupDetails.length > 0).sort((a, b) => a.VersionGroupDetails[0].LevelLearnedAt - b.VersionGroupDetails[0].LevelLearnedAt);
        console.log(lvlUpMoveData);
    
    </script>
    

<!-- Button trigger modal -->
<button type="button" class="btn btn-primary mx-1" data-bs-toggle="modal" data-bs-target="#{modalName}">
    Moves
</button>

  <!-- Modal -->
  <div class="modal fade" id="{modalName}" tabindex="-1" aria-labelledby="{modalName}-Label" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title text-capitalize" id="{modalName}-Label">Moves: {data.name}</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        <div class="modal-body">
            <div class="container px-5">
                <h4>Learned Through Level Up</h4>
                <table class="table">
                    <thead>
                        <tr>
                            <th scope="col">Move Name</th>
                            <th scope="col">Level Learned At</th>
                        </tr>
                    </thead>
                    <tbody>
                        {#each lvlUpMoveData as move}
                            <tr>
                                <td>{move.Move.Name}</td>
                                <td>{move.VersionGroupDetails[0].LevelLearnedAt}</td>
                            </tr>
                        {/each}
                    </tbody>
                </table>
            </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-primary" data-bs-dismiss="modal">Close</button>
        </div>
      </div>
    </div>
  </div>
  </div>
