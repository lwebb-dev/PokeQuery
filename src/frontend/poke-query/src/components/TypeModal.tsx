import React, { useMemo } from 'react';
import TypeChiclet from './TypeChiclet';

interface TypeModalProps {
  pkmnType: any;
}

const TypeModal: React.FC<TypeModalProps> = ({ pkmnType }) => {
  const typeName = pkmnType?.name || '';
  const modalName = `typeModal-${typeName}`;
  const modalLabel = `${modalName}-Label`;
  const typeData = useMemo(() => {
    try {
      const raw = sessionStorage.getItem('typeData');
      if (!raw) return [];
      const arr = JSON.parse(raw);
      return arr.map((x: string) => typeof x === 'string' ? JSON.parse(x) : x);
    } catch {
      return [];
    }
  }, []);
  const damageRelations = useMemo(() => {
    return typeData.find((x: any) => x.name === typeName)?.damage_relations || {};
  }, [typeData, typeName]);

  // Helper for each damage relation type
  const renderTypeRow = (label: string, arr: any[] = []) =>
    arr.length > 0 && (
      <>
        <h6>{label}</h6>
        <div className="d-flex flex-wrap justify-content-center mb-2">
          {arr.map((type, i) => (
            <TypeChiclet key={type.name + i} typeName={type.name} isStatic={true} />
          ))}
        </div>
      </>
    );

  return (
    <>
      {/* Button trigger modal */}
      <button
        type="button"
        className="btn btn-link p-0 border-0 align-baseline"
        data-bs-toggle="modal"
        data-bs-target={`#${modalName}`}
        style={{ textDecoration: 'none' }}
      >
        <TypeChiclet typeName={typeName} isStatic={false} />
      </button>

      {/* Modal */}
      <div className="modal fade" id={modalName} tabIndex={-1} aria-labelledby={modalLabel} aria-hidden="true">
        <div className="modal-dialog modal-dialog-centered modal-dialog-scrollable">
          <div className="modal-content">
            <div className="modal-header">
              <h6 className="modal-title text-capitalize" id={modalLabel}>Type: {typeName}</h6>
              <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div className="modal-body">
              <div className="container px-3">
                <div className="container title-border mb-3 p-3">
                  <h4>Offensive</h4>
                  {renderTypeRow('Strong Against:', damageRelations.double_damage_to)}
                  {renderTypeRow('Weak Against:', damageRelations.half_damage_to)}
                  {renderTypeRow('Resisted By:', damageRelations.no_damage_to)}
                </div>
                <div className="container title-border mb-3 p-3">
                  <h4>Defensive</h4>
                  {renderTypeRow('Strong Against:', damageRelations.half_damage_from)}
                  {renderTypeRow('Weak Against:', damageRelations.double_damage_from)}
                  {renderTypeRow('Immune To:', damageRelations.no_damage_from)}
                </div>
              </div>
            </div>
            <div className="modal-footer">
              <button type="button" className="btn btn-primary" data-bs-dismiss="modal">Close</button>
            </div>
          </div>
        </div>
      </div>
      <style>{`
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
      `}</style>
    </>
  );
};

export default TypeModal;
