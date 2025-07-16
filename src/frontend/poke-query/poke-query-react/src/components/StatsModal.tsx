import React from 'react';

interface StatsModalProps {
  data: any;
}

const statNames: Record<string, string> = {
  hp: 'HP',
  attack: 'Attack',
  defense: 'Defense',
  'special-attack': 'Sp. Atk',
  'special-defense': 'Sp. Def',
  speed: 'Speed',
};
const MAX_STAT_VALUE = 255;

const getStatPercent = (statValue: number) => {
  return statValue === 255 ? 100 : Math.round((statValue / MAX_STAT_VALUE) * 100);
};
const getStatColorClass = (statValue: number) => {
  if (statValue < 60) return 'danger';
  if (statValue < 120) return 'warning';
  if (statValue < 180) return 'success';
  return 'info';
};

const StatsModal: React.FC<StatsModalProps> = ({ data }) => {
  const modalName = `statModal-${data.name}`;
  const modalLabel = `${modalName}-Label`;
  const statData = data.stats || [];
  return (
    <>
      {/* Button trigger modal */}
      <button
        type="button"
        className="btn btn-primary mx-1"
        data-bs-toggle="modal"
        data-bs-target={`#${modalName}`}
      >
        Stats
      </button>

      {/* Modal */}
      <div className="modal fade" id={modalName} tabIndex={-1} aria-labelledby={modalLabel} aria-hidden="true">
        <div className="modal-dialog modal-dialog-centered">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title text-capitalize" id={modalLabel}>Stats: {data.name}</h5>
              <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div className="modal-body">
              <div className="container px-5">
                {statData.map((stat: any, idx: number) => (
                  <div className="row" key={stat.stat.name + '-' + idx}>
                    <div className="col-4 text-end">
                      {statNames[stat.stat.name] || stat.stat.name}:
                    </div>
                    <div className="col-8">
                      <div className="row">
                        <div className="col-2 p-0 text-end">
                          {stat.base_stat}
                        </div>
                        <div className="col-10">
                          <div className="progress">
                            <div
                              className={`progress-bar bg-${getStatColorClass(stat.base_stat)}`}
                              role="progressbar"
                              style={{ width: `${getStatPercent(stat.base_stat)}%` }}
                              aria-valuenow={stat.base_stat}
                              aria-valuemin={0}
                              aria-valuemax={MAX_STAT_VALUE}
                            ></div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            </div>
            <div className="modal-footer">
              <button type="button" className="btn btn-primary" data-bs-dismiss="modal">Close</button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default StatsModal;
