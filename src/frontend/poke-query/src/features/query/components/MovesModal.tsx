import React, { useMemo, useState } from 'react';
import styles from './MovesModal.module.css';
import classNames from 'classnames';

interface MovesModalProps {
  data: any;
}

const getIdFromUrl = (url: string) => {
  const match = url.match(/\/(\d+)(?!.*\d)/);
  return match ? Number(match[1]) : null;
};

const MovesModal: React.FC<MovesModalProps> = ({ data }) => {
  const modalName = `movesModal-${data.name}`;
  const modalLabel = `${modalName}-Label`;
  const moveData = data.moves || [];
  const [selectedVersion, setSelectedVersion] = useState<string>('');
  const [versionGroups, setVersionGroups] = useState<string[]>([]);
  const [lvlUpMoveData, setLvlUpMoveData] = useState<any[]>([]);
  const [eggMoveData, setEggMoveData] = useState<any[]>([]);
  const [tutorMoveData, setTutorMoveData] = useState<any[]>([]);
  const [machineMoveDict, setMachineMoveDict] = useState<any[]>([]);

  // Session data
  const sessionVersionGroups = useMemo(() => {
    try {
      const arr = JSON.parse(sessionStorage.getItem('versionGroupData') || '[]');
      return arr.map((x: string) => typeof x === 'string' ? JSON.parse(x) : x);
    } catch {
      return [];
    }
  }, []);
  const sessionMachineData = useMemo(() => {
    try {
      const arr = JSON.parse(sessionStorage.getItem('machineData') || '[]');
      return arr.map((x: string) => typeof x === 'string' ? JSON.parse(x) : x);
    } catch {
      return [];
    }
  }, []);

  // Version group extraction
  React.useEffect(() => {
    const vgs: string[] = [];
    moveData.forEach((x: any) => {
      x.version_group_details.forEach((y: any) => vgs.push(y.version_group.name));
    });
    const unique = [...new Set(vgs)].sort((a, b) => {
      const aId = sessionVersionGroups.find((x: any) => x.name === a)?.id || 0;
      const bId = sessionVersionGroups.find((x: any) => x.name === b)?.id || 0;
      return aId - bId;
    });
    setVersionGroups(unique);
    setSelectedVersion(unique[0] || '');
  }, [moveData, sessionVersionGroups]);

  // Filtering helpers
  const filterBySelectedVersion = (data: any[]) =>
    data.filter((x: any) => {
      x.version_group_details = x.version_group_details.filter(
        (vgd: any) => vgd.version_group.name === selectedVersion
      );
      return x.version_group_details.length > 0;
    });
  const filterByLearnMethod = (data: any[], learnMethod: string) =>
    data.filter((x: any) => {
      x.version_group_details = x.version_group_details.filter(
        (vgd: any) => vgd.move_learn_method.name === learnMethod
      );
      return x.version_group_details.length > 0;
    });

  // Machine lookup
  const getMachine = (move: any) => {
    const machine = sessionMachineData.find(
      (x: any) =>
        getIdFromUrl(x.move.url) === getIdFromUrl(move.move.url) &&
        x.version_group.name === move.version_group_details[0].version_group.name
    );
    return machine?.item?.name || '';
  };

  // Load move data by version
  React.useEffect(() => {
    if (!selectedVersion) return;
    let versionMoveData = filterBySelectedVersion(structuredClone(moveData));
    // For egg moves, use base data if available
    let baseMoveData = versionMoveData;
    setLvlUpMoveData(filterByLearnMethod(structuredClone(versionMoveData), 'level-up'));
    const machineMoves = filterByLearnMethod(structuredClone(versionMoveData), 'machine');
    setEggMoveData(filterByLearnMethod(structuredClone(baseMoveData), 'egg'));
    setTutorMoveData(filterByLearnMethod(structuredClone(versionMoveData), 'tutor'));
    // Build machineMoveDict
    setMachineMoveDict(
      machineMoves.map((x: any) => ({ machine: getMachine(x), data: x }))
        .sort((a: any, b: any) => a.machine.localeCompare(b.machine))
    );
    setLvlUpMoveData((arr) => arr.sort((a, b) => a.version_group_details[0].level_learned_at - b.version_group_details[0].level_learned_at));
  }, [selectedVersion, moveData, sessionMachineData]);

  return (
    <>
      {/* Button trigger modal */}
      <button
        type="button"
        className="btn btn-primary mx-1"
        data-bs-toggle="modal"
        data-bs-target={`#${modalName}`}
      >
        Moves
      </button>

      {/* Modal */}
      <div className="modal fade" id={modalName} tabIndex={-1} aria-labelledby={modalLabel} aria-hidden="true">
        <div className="modal-dialog modal-lg modal-dialog-centered modal-dialog-scrollable">
          <div className="modal-content">
            <div className="modal-header">
              <h5 className="modal-title text-capitalize" id={modalLabel}>
                Moves: {data.name}
              </h5>
              <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close" />
            </div>
            <div className="modal-body">
              <div className="container modal-container px-5">
                <label htmlFor="version-select" className={"my-0 " + styles.labelBlock}>Version(s):</label>
                <select
                  id="version-select"
                  className="form-select-lg my-3 mt-1 text-capitalize"
                  aria-label="version group select"
                  value={selectedVersion}
                  onChange={e => setSelectedVersion(e.target.value)}
                >
                  {versionGroups.map(version => (
                    <option className="text-capitalize" value={version} key={version}>
                      {version.replaceAll('-', ' ')}
                    </option>
                  ))}
                </select>

                <div className="accordion" id={`moveModalAccordion-${modalName}`}>
                  {/* Level Up */}
                  <div className="accordion-item">
                    <h2 className="accordion-header" id={`panelsStayOpen-headingOne-${modalName}`}>
                      <button className="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target={`#panelsStayOpen-collapseOne-${modalName}`} aria-expanded="true" aria-controls={`panelsStayOpen-collapseOne-${modalName}`}>
                        Level Up
                      </button>
                    </h2>
                    <div id={`panelsStayOpen-collapseOne-${modalName}`} className="accordion-collapse collapse show" aria-labelledby={`panelsStayOpen-headingOne-${modalName}`}>
              <div className={classNames('accordion-body', styles.scrollingTable)}>
                        <table className="table table-striped table-sm">
                          <thead>
                            <tr>
                              <th scope="col">Move</th>
                              <th scope="col">Level</th>
                            </tr>
                          </thead>
                          <tbody>
                            {lvlUpMoveData.map((move, i) =>
                              move.version_group_details.map((versionGroup: any, j: number) => (
                                <tr key={move.move.name + '-' + i + '-' + j}>
                                  <td className="align-middle text-capitalize">{move.move.name.replace('-', ' ')}</td>
                                  <td className="align-middle">{versionGroup.level_learned_at}</td>
                                </tr>
                              ))
                            )}
                          </tbody>
                        </table>
                      </div>
                    </div>
                  </div>
                  {/* Machine */}
                  <div className="accordion-item">
                    <h2 className="accordion-header" id={`panelsStayOpen-headingTwo-${modalName}`}>
                      <button className="accordion-button collapsed" type="button" id="machineButton" data-bs-toggle="collapse" data-bs-target={`#panelsStayOpen-collapseTwo-${modalName}`} aria-expanded="false" aria-controls={`panelsStayOpen-collapseTwo-${modalName}`}>
                        Machine
                      </button>
                    </h2>
                    <div id={`panelsStayOpen-collapseTwo-${modalName}`} className="accordion-collapse collapse" aria-labelledby={`panelsStayOpen-headingTwo-${modalName}`}>
              <div className={classNames('accordion-body', styles.scrollingTable)}>
                        <table className="table table-striped">
                          <thead>
                            <tr>
                              <th scope="col">Move</th>
                              <th scope="col">TM/HM</th>
                            </tr>
                          </thead>
                          <tbody>
                            {machineMoveDict.map((move, i) => (
                              <tr key={move.data.move.name + '-' + i}>
                                <td className="text-capitalize">{move.data.move.name.replace('-', ' ')}</td>
                                <td>{move.machine.toUpperCase()}</td>
                              </tr>
                            ))}
                          </tbody>
                        </table>
                      </div>
                    </div>
                  </div>
                  {/* Egg */}
                  <div className="accordion-item">
                    <h2 className="accordion-header" id={`panelsStayOpen-headingThree-${modalName}`}>
                      <button className="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target={`#panelsStayOpen-collapseThree-${modalName}`} aria-expanded="false" aria-controls={`panelsStayOpen-collapseThree-${modalName}`}>
                        Egg
                      </button>
                    </h2>
                    <div id={`panelsStayOpen-collapseThree-${modalName}`} className="accordion-collapse collapse" aria-labelledby={`panelsStayOpen-headingThree-${modalName}`}>
              <div className={classNames('accordion-body', styles.scrollingTable)}>
                        <table className="table table-striped">
                          <thead>
                            <tr>
                              <th scope="col">Move</th>
                            </tr>
                          </thead>
                          <tbody>
                            {eggMoveData.map((move, i) => (
                              <tr key={move.move.name + '-' + i}>
                                <td className="text-capitalize">{move.move.name.replace('-', ' ')}</td>
                              </tr>
                            ))}
                          </tbody>
                        </table>
                      </div>
                    </div>
                  </div>
                  {/* Tutor */}
                  <div className="accordion-item">
                    <h2 className="accordion-header" id={`panelsStayOpen-headingFour-${modalName}`}>
                      <button className="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target={`#panelsStayOpen-collapseFour-${modalName}`} aria-expanded="false" aria-controls={`panelsStayOpen-collapseFour-${modalName}`}>
                        Tutor
                      </button>
                    </h2>
                    <div id={`panelsStayOpen-collapseFour-${modalName}`} className="accordion-collapse collapse" aria-labelledby={`panelsStayOpen-headingFour-${modalName}`}>
              <div className={classNames('accordion-body', styles.scrollingTable)}>
                        <table className="table table-striped">
                          <thead>
                            <tr>
                              <th scope="col">Move</th>
                            </tr>
                          </thead>
                          <tbody>
                            {tutorMoveData.map((move, i) => (
                              <tr key={move.move.name + '-' + i}>
                                <td className="text-capitalize">{move.move.name.replace('-', ' ')}</td>
                              </tr>
                            ))}
                          </tbody>
                        </table>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div className="modal-footer">
              <button type="button" className="btn btn-primary" data-bs-dismiss="modal">
                Close
              </button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default MovesModal;
