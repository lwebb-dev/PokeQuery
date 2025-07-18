<script>
// @ts-nocheck
    import { statNames } from '../../common';

    export let data;
    const MAX_STAT_VALUE = 255;
    let statData = data.stats;
    let modalName = `statModal-${data.name}`;

    const getStatPercent = (statValue) => {
        return statValue === 255 ? 100 : parseInt(((statValue / MAX_STAT_VALUE) * 100).toPrecision(2));
    };

    const getStatColorClass = (statValue) => {

        if (statValue < 60)
            return "danger";
        
        if (statValue < 120)
            return "warning";

        if (statValue < 180)
            return "success";

        return "info";
    }

</script>

<!-- Button trigger modal -->
<button type="button" class="btn btn-primary mx-1" data-bs-toggle="modal" data-bs-target="#{modalName}">
    Stats
  </button>
  
  <!-- Modal -->
  <div class="modal fade" id="{modalName}" tabindex="-1" aria-labelledby="{modalName}-Label" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title text-capitalize" id="{modalName}-Label">Stats: {data.name}</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        <div class="modal-body">
          <div class="container px-5">
            {#each statData as stat}
                <div class="row">
                    <div class="col-4 text-end">
                        {statNames[stat.stat.name]}:
                    </div>
                    <div class="col-8">
                        <div class="row">
                            <div class="col-2 p-0 text-end">
                                {stat.base_stat}
                            </div>
                            <div class="col-10">
                                <div class="progress">
                                    <div 
                                    class="progress-bar bg-{getStatColorClass(stat.base_stat)}" 
                                    role="progressbar" 
                                    style="width: {getStatPercent((stat.base_stat))}%"
                                    aria-valuenow="{stat.base_stat}"
                                    aria-valuemin="0"
                                    aria-valuemax="{MAX_STAT_VALUE}"
                                    ></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            {/each}
          </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-primary" data-bs-dismiss="modal">Close</button>
        </div>
      </div>
    </div>
  </div>