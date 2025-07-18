import React from 'react';
// TODO: Import statNames from common if needed

interface NatureCardProps {
  data: any;
}

const cardNatureStyle = {
  borderColor: "#befad5",
  borderWidth: "0.35em",
  width: "15rem",
  height: "21rem"
};

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
    <div style={cardNatureStyle} className="card mw-20 m-2">
      <div className="card-body">
        <h2 className="mt-2 card-title text-capitalize text-center">
          {data.name}
        </h2>
        <div className="mt-5 nature-stats">
          {data.increased_stat === null && data.decreased_stat === null ? (
            <h3>Neutral</h3>
          ) : (
            <>
              <h3>{statNames[data.increased_stat?.name]} <i className="bi bi-arrow-up-circle-fill" style={{ color: '#ff0000' }}></i></h3>
              <h3>{statNames[data.decreased_stat?.name]} <i className="bi bi-arrow-down-circle-fill" style={{ color: '#0000ff' }}></i></h3>
            </>
          )}
        </div>
      </div>
    </div>
  );
};

export default NatureCard;
