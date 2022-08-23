<script>
  // @ts-nocheck
  import { onMount } from 'svelte';
  import { getIdFromUrl, fetchJson } from '../../common';

  export let data;

  let basePkmnData, baseMoveData;

  let modalName = `movesModal-${data.name}`;
  let baseUri = process.env.API_BASE_URI;
  let moveData = data.moves;
  const sessionVersionGroups = JSON.parse(sessionStorage.versionGroupData);
  const sessionMachineData = JSON.parse(sessionStorage.machineData);

  for (let i = 0; i < sessionVersionGroups.length; i++) {
    sessionVersionGroups[i] = JSON.parse(sessionVersionGroups[i]);
  }

  let versionGroups = [];
  let machineMoveDict = [];
  let pkmnSpecies, basePkmnSpecies;
  let selectedVersion;

  let lvlUpMoveData = [], machineMoveData = [], eggMoveData = [], tutorMoveData = []

  const getBasePkmnSpecies = async (species) => {
    if (species.evolves_from_species === null) {
      return await species;
    }

    const evolvesFromSpecies = await fetchJson(baseUri, "pokemon-species", getIdFromUrl(species.evolves_from_species.url));
    return getBasePkmnSpecies(evolvesFromSpecies);
  }

  const getBasePkmnData = async (id) => {
    if (basePkmnSpecies.id === pkmnSpecies.id) {
      return await data;
    }

    return await fetchJson(baseUri, "pokemon", basePkmnSpecies.id);
  }

  const getMachine = (move) => {
    const machine = sessionMachineData.find((x) => { 
      const moveId = getIdFromUrl(move.move.url);
      const machineJsonObject = JSON.parse(x);
      const machineMoveUrlId = getIdFromUrl(machineJsonObject.move.url);
      return machineMoveUrlId === moveId && machineJsonObject.version_group.name === move.version_group_details[0].version_group.name;
     });

    return JSON.parse(machine).item.name;
  };

  const loadMoveDataByVersion = () => {

    let moveDataClone = structuredClone(moveData);
    let baseMoveDataClone = structuredClone(baseMoveData);

    let versionMoveData = moveDataClone.filter(
      (x) =>
        (x.version_group_details = x.version_group_details.filter(
          (vgd) => vgd.version_group.name === selectedVersion
        ))
    );

    let baseVersionMoveData = baseMoveDataClone.filter(
      (x) =>
        (x.version_group_details = x.version_group_details.filter(
          (vgd) => vgd.version_group.name === selectedVersion
        ))
    );

    lvlUpMoveData = structuredClone(versionMoveData);
    lvlUpMoveData.forEach(
      (x) =>
        (x.version_group_details = x.version_group_details.filter(
          (vgd) => vgd.move_learn_method.name === "level-up"
        ))
    );
    lvlUpMoveData = lvlUpMoveData
      .filter((x) => x.version_group_details.length > 0)
      .sort(
        (a, b) =>
          a.version_group_details[0].level_learned_at -
          b.version_group_details[0].level_learned_at
      );

    machineMoveData = structuredClone(versionMoveData);
    machineMoveDict = [];
    machineMoveData.forEach(
      (x) =>
        (x.version_group_details = x.version_group_details.filter(
          (vgd) => vgd.move_learn_method.name === "machine"
        ))
    );
    machineMoveData = machineMoveData.filter(
      (x) => x.version_group_details.length > 0
    );
    machineMoveData.forEach((x) => {
      machineMoveDict.push({machine: getMachine(x), data: x});
    });
    machineMoveDict.sort((a, b) => a.machine.localeCompare(b.machine) );

    eggMoveData = structuredClone(baseVersionMoveData);
    eggMoveData.forEach(
      (x) =>
        (x.version_group_details = x.version_group_details.filter(
          (vgd) => vgd.move_learn_method.name === "egg"
        ))
    );
    eggMoveData = eggMoveData.filter((x) => x.version_group_details.length > 0);

    tutorMoveData = structuredClone(versionMoveData);
    tutorMoveData.forEach(
      (x) =>
        (x.version_group_details = x.version_group_details.filter(
          (vgd) => vgd.move_learn_method.name === "tutor"
        ))
    );
    tutorMoveData = tutorMoveData.filter(
      (x) => x.version_group_details.length > 0
    );
  };

  onMount(async () => {
    pkmnSpecies = await fetchJson(baseUri, "pokemon-species", data.id);
    basePkmnSpecies = await getBasePkmnSpecies(pkmnSpecies);
    basePkmnData = await getBasePkmnData();
    baseMoveData = basePkmnData.moves;

    moveData.forEach((x) => {
    x.version_group_details.forEach((y) =>
      versionGroups.push(y.version_group.name)
    );
    });


    versionGroups = [...new Set(versionGroups)].sort(
      (a, b) =>
        sessionVersionGroups.find((x) => x.name === a).id -
        sessionVersionGroups.find((x) => x.name === b).id
    );

    selectedVersion = versionGroups[0];
    loadMoveDataByVersion();
  });
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
              <option class="text-capitalize" value={version}>{version.replaceAll('-', ' ')}</option>
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
                        {#each move.version_group_details as versionGroup}
                          <tr>
                            <td class="text-capitalize">{move.move.name.replace('-', ' ')}</td>
                            <td>{versionGroup.level_learned_at}</td>
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
                  id="machineButton"
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
                      {#each machineMoveDict as move}
                        <tr>
                          <td class="text-capitalize">{move.data.move.name.replace('-', ' ')}</td>
                          <td>{move.machine.toUpperCase()}</td>
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
                          <td class="text-capitalize">{move.move.name.replace('-', ' ')}</td>
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
                          <td class="text-capitalize">{move.move.name.replace('-', ' ')}</td>
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
