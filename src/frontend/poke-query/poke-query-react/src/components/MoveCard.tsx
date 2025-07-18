import React from 'react';
import TypeModal from './TypeModal';

interface MoveCardProps {
  data: any;
}

const cardMoveStyle = {
  borderColor: "#bee4fa",
  borderWidth: "0.35em",
  width: "240px",
  height: "326px"
};

const moveAttributeStyle = {
  margin: '0'
};

const handleNull = (value: any) => {
  if (value === null || value < 5) return '--';
  return value;
};

const interpolateEffectChance = (effect: string, chance: any) => {
  return effect.replaceAll('$effect_chance%', `${chance}%`);
};

const MoveCard: React.FC<MoveCardProps> = ({ data }) => {
  return (
    <div style={cardMoveStyle} className="card mw-20 m-2">
      <div className="card-body">
        <h4 className="card-title text-capitalize text-center">
          {data.name?.replaceAll('-', ' ')}
        </h4>
        <div className="d-flex justify-content-center">
          <TypeModal pkmnType={data.type} />
        </div>
        <div className="d-flex flex-wrap justify-content-center my-2" style={{ columnGap: '1rem' }}>
              <p style={{ margin: 0 }}><strong>Power:</strong> {handleNull(data.power)}</p>
              <p style={{ margin: 0 }}><strong>Accuracy:</strong> {handleNull(data.accuracy)}</p>
              <p style={{ margin: 0 }}><strong>PP:</strong> {handleNull(data.pp)}</p>
              <p className="text-capitalize" style={{ margin: 0 }}><strong>Type:</strong> {handleNull(data.damage_class?.name)}</p>
        </div>
        <div className="d-flex flex-wrap justify-content-center">
          {data.effect_entries && data.effect_entries[0] && (
            <p className="card-text text-wrap">
              {interpolateEffectChance(
                data.effect_entries[0].short_effect,
                data.effect_chance
              )}
            </p>
          )}
        </div>
      </div>
    </div>
  );
};

export default MoveCard;
