import React from 'react';
import TypeModal from './TypeModal';

interface MoveCardProps {
  data: any;
}

const handleNull = (value: any) => {
  if (value === null || value < 5) return '--';
  return value;
};

const interpolateEffectChance = (effect: string, chance: any) => {
  return effect.replaceAll('$effect_chance%', `${chance}%`);
};

const MoveCard: React.FC<MoveCardProps> = ({ data }) => {
  return (
    <div className="card mw-20 m-2 border-4 border-[#bee4fa] w-60 h-[21rem]">
      <div className="card-body p-4">
        <h4 className="card-title text-capitalize text-center text-lg font-bold">
          {data.name?.replaceAll('-', ' ')}
        </h4>
        <div className="flex justify-center gap-2 my-2">
          <TypeModal pkmnType={data.type} />
        </div>
        <div className="flex flex-wrap justify-center my-2 gap-4">
          <p className="move-attribute text-sm"><strong>Power:</strong> {handleNull(data.power)}</p>
          <p className="move-attribute text-sm"><strong>Accuracy:</strong> {handleNull(data.accuracy)}</p>
          <p className="move-attribute text-sm"><strong>PP:</strong> {handleNull(data.pp)}</p>
          <p className="move-attribute text-sm text-capitalize"><strong>Type:</strong> {handleNull(data.damage_class?.name)}</p>
        </div>
        <div className="flex flex-wrap justify-center">
          {data.effect_entries && data.effect_entries[0] && (
            <p className="card-text text-wrap text-sm mt-2">
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
