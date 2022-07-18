<script>
  // @ts-nocheck

  export let data;

  let modalName = `movesModal-${data.name}`;
  let moveData = data.json.Moves;
  let sessionVersionGroups = JSON.parse(sessionStorage.versionGroups);
  let versionGroups = [];
  moveData.forEach((x) =>
    x.VersionGroupDetails.forEach((y) =>
      versionGroups.push(y.VersionGroup.Name)
    )
  );
  versionGroups = [...new Set(versionGroups)].sort(
    (a, b) =>
      sessionVersionGroups.find((x) => x.name === a).id -
      sessionVersionGroups.find((x) => x.name === b).id
  );

  let lvlUpMoveData;
  let machineMoveData;
  let eggMoveData;
  let tutorMoveData;

  const loadMoveDataByVersion = () => {
    let moveDataClone = structuredClone(moveData);

    let versionMoveData = moveDataClone.filter(
      (x) =>
        (x.VersionGroupDetails = x.VersionGroupDetails.filter(
          (vgd) => vgd.VersionGroup.Name === selectedVersion
        ))
    );

    lvlUpMoveData = structuredClone(versionMoveData);
    lvlUpMoveData.forEach(
      (x) =>
        (x.VersionGroupDetails = x.VersionGroupDetails.filter(
          (vgd) => vgd.MoveLearnMethod.Name === "level-up"
        ))
    );
    lvlUpMoveData = lvlUpMoveData
      .filter((x) => x.VersionGroupDetails.length > 0)
      .sort(
        (a, b) =>
          a.VersionGroupDetails[0].LevelLearnedAt -
          b.VersionGroupDetails[0].LevelLearnedAt
      );

    machineMoveData = structuredClone(versionMoveData);
    machineMoveData.forEach(
      (x) =>
        (x.VersionGroupDetails = x.VersionGroupDetails.filter(
          (vgd) => vgd.MoveLearnMethod.Name === "machine"
        ))
    );
    machineMoveData = machineMoveData.filter(
      (x) => x.VersionGroupDetails.length > 0
    );

    eggMoveData = structuredClone(versionMoveData);
    eggMoveData.forEach(
      (x) =>
        (x.VersionGroupDetails = x.VersionGroupDetails.filter(
          (vgd) => vgd.MoveLearnMethod.Name === "egg"
        ))
    );
    eggMoveData = eggMoveData.filter((x) => x.VersionGroupDetails.length > 0);

    tutorMoveData = structuredClone(versionMoveData);
    tutorMoveData.forEach(
      (x) =>
        (x.VersionGroupDetails = x.VersionGroupDetails.filter(
          (vgd) => vgd.MoveLearnMethod.Name === "tutor"
        ))
    );
    tutorMoveData = tutorMoveData.filter(
      (x) => x.VersionGroupDetails.length > 0
    );
  };

  let selectedVersion = versionGroups[0];
  loadMoveDataByVersion();
</script>

<!-- Button trigger modal -->
<button
  type="button"
  class="btn btn-primary mx-1"
  data-bs-toggle="modal"
  data-bs-target="#{modalName}"
>
  Moves
</button>

<!-- Modal -->
<div
  class="modal fade"
  id={modalName}
  tabindex="-1"
  aria-labelledby="{modalName}-Label"
  aria-hidden="true"
>
  <div
    class="modal-dialog modal-lg modal-dialog-centered modal-dialog-scrollable"
  >
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title text-capitalize" id="{modalName}-Label">
          Moves: {data.name}
        </h5>
        <button
          type="button"
          class="btn-close"
          data-bs-dismiss="modal"
          aria-label="Close"
        />
      </div>
      <div class="modal-body">
        <div class="container modal-container px-5">
          <label for="version-select" class="my-0" style="display: block;">Version(s):</label>
          <select
            id="version-select"
            class="form-select-lg my-3 mt-1 text-capitalize"
            aria-label="version group select"
            bind:value={selectedVersion}
            on:change={() => loadMoveDataByVersion()}
          >
            {#each versionGroups as version}
              <option class="text-capitalize" value={version}>{version.replace('-', ' ')}</option>
            {/each}
          </select>

          <div class="accordion" id="moveModalAccordion-{modalName}">
            <div class="accordion-item">
              <h2
                class="accordion-header"
                id="panelsStayOpen-headingOne-{modalName}"
              >
                <button
                  class="accordion-button"
                  type="button"
                  data-bs-toggle="collapse"
                  data-bs-target="#panelsStayOpen-collapseOne-{modalName}"
                  aria-expanded="true"
                  aria-controls="panelsStayOpen-collapseOne-{modalName}"
                >
                  Level Up
                </button>
              </h2>
              <div
                id="panelsStayOpen-collapseOne-{modalName}"
                class="accordion-collapse collapse show"
                aria-labelledby="panelsStayOpen-headingOne-{modalName}"
              >
                <div class="accordion-body scrolling-table">
                  <table class="table">
                    <thead>
                      <tr>
                        <th scope="col">Move</th>
                        <th scope="col">Level</th>
                      </tr>
                    </thead>
                    <tbody>
                      {#each lvlUpMoveData as move}
                        {#each move.VersionGroupDetails as versionGroup}
                          <tr>
                            <td class="text-capitalize">{move.Move.Name.replace('-', ' ')}</td>
                            <td>{versionGroup.LevelLearnedAt}</td>
                          </tr>
                        {/each}
                      {/each}
                    </tbody>
                  </table>
                </div>
              </div>
            </div>
            <div class="accordion-item">
              <h2
                class="accordion-header"
                id="panelsStayOpen-headingTwo-{modalName}"
              >
                <button
                  class="accordion-button collapsed"
                  type="button"
                  data-bs-toggle="collapse"
                  data-bs-target="#panelsStayOpen-collapseTwo-{modalName}"
                  aria-expanded="false"
                  aria-controls="panelsStayOpen-collapseTwo-{modalName}"
                >
                  Machine
                </button>
              </h2>
              <div
                id="panelsStayOpen-collapseTwo-{modalName}"
                class="accordion-collapse collapse"
                aria-labelledby="panelsStayOpen-headingTwo-{modalName}"
              >
                <div class="accordion-body scrolling-table">
                  <table class="table">
                    <thead>
                      <tr>
                        <th scope="col">Move</th>
                        <th scope="col">TM/HM</th>
                      </tr>
                    </thead>
                    <tbody>
                      {#each machineMoveData as move}
                        <tr>
                          <td class="text-capitalize">{move.Move.Name.replace('-', ' ')}</td>
                          <td>--</td>
                        </tr>
                      {/each}
                    </tbody>
                  </table>
                </div>
              </div>
            </div>
            <div class="accordion-item">
              <h2
                class="accordion-header"
                id="panelsStayOpen-headingThree-{modalName}"
              >
                <button
                  class="accordion-button collapsed"
                  type="button"
                  data-bs-toggle="collapse"
                  data-bs-target="#panelsStayOpen-collapseThree-{modalName}"
                  aria-expanded="false"
                  aria-controls="panelsStayOpen-collapseThree-{modalName}"
                >
                  Egg
                </button>
              </h2>
              <div
                id="panelsStayOpen-collapseThree-{modalName}"
                class="accordion-collapse collapse"
                aria-labelledby="panelsStayOpen-headingThree-{modalName}"
              >
                <div class="accordion-body scrolling-table">
                  <table class="table">
                    <thead>
                      <tr>
                        <th scope="col">Move</th>
                      </tr>
                    </thead>
                    <tbody>
                      {#each eggMoveData as move}
                        <tr>
                          <td class="text-capitalize">{move.Move.Name.replace('-', ' ')}</td>
                        </tr>
                      {/each}
                    </tbody>
                  </table>
                </div>
              </div>
            </div>
            <div class="accordion-item">
              <h2
                class="accordion-header"
                id="panelsStayOpen-headingFour-{modalName}"
              >
                <button
                  class="accordion-button collapsed"
                  type="button"
                  data-bs-toggle="collapse"
                  data-bs-target="#panelsStayOpen-collapseFour-{modalName}"
                  aria-expanded="false"
                  aria-controls="panelsStayOpen-collapseFour-{modalName}"
                >
                  Tutor
                </button>
              </h2>
              <div
                id="panelsStayOpen-collapseFour-{modalName}"
                class="accordion-collapse collapse"
                aria-labelledby="panelsStayOpen-headingFour-{modalName}"
              >
                <div class="accordion-body scrolling-table">
                  <table class="table">
                    <thead>
                      <tr>
                        <th scope="col">Move</th>
                      </tr>
                    </thead>
                    <tbody>
                      {#each tutorMoveData as move}
                        <tr>
                          <td class="text-capitalize">{move.Move.Name.replace('-', ' ')}</td>
                        </tr>
                      {/each}
                    </tbody>
                  </table>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-primary" data-bs-dismiss="modal"
            >Close</button
          >
        </div>
      </div>
    </div>
  </div>
</div>

<style>
  .scrolling-table {
    max-height: 25rem;
    width: 100%;
    overflow: auto;
    display: inline-block;
  }
</style>
