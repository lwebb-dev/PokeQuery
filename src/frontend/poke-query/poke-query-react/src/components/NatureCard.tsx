import React from 'react';
// TODO: Import statNames from common if needed

interface NatureCardProps {
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

const NatureCard: React.FC<NatureCardProps> = ({ data }) => {
  return (
    <div className="card mw-20 m-2 border-4 border-[#befad5] w-60 h-[21rem]">
      <div className="card-body p-4">
        <h2 className="mt-2 card-title text-capitalize text-center text-lg font-bold">
          {data.name}
        </h2>
        <div className="mt-5 flex flex-col items-center">
          {data.increased_stat === null && data.decreased_stat === null ? (
            <h3 className="text-base">Neutral</h3>
          ) : (
            <>
              <h3 className="text-base flex items-center gap-2">
                {statNames[data.increased_stat?.name]}
                <span role="img" aria-label="up" style={{ color: '#ff0000' }}>⬆️</span>
              </h3>
              <h3 className="text-base flex items-center gap-2">
                {statNames[data.decreased_stat?.name]}
                <span role="img" aria-label="down" style={{ color: '#0000ff' }}>⬇️</span>
              </h3>
            </>
          )}
        </div>
      </div>
    </div>
  );
};

export default NatureCard;
