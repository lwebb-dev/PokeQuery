import React, { useState } from 'react';
import TypeModal from './TypeModal';
import MovesModal from './MovesModal';
import StatsModal from './StatsModal';

interface PokemonCardProps {
  data: any;
}

const cardPkmnStyle = {
    borderColor: "#fc8686",
    borderWidth: "0.35em",
    width: "15rem",
    height: "21rem"
  };

const stripGarbageSpriteText = (text?: string) => {
  return text?.replace(
    'https://raw.githubusercontent.com/PokeAPI/sprites/master/https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites',
    'https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites'
  );
};

const PokemonCard: React.FC<PokemonCardProps> = ({ data }) => {
  const [shinyBtnSelected, setShinyBtnSelected] = useState(false);

  return (
    <div style={cardPkmnStyle} className="card mw-20 m-2 position-relative">
      <button
        className="shiny-btn position-absolute"
        style={{ top: '0.5rem', right: '0.5rem', background: 'none', border: 'none', padding: 0, outline: 'none', boxShadow: 'none' }}
        onClick={() => setShinyBtnSelected(!shinyBtnSelected)}
        aria-label="Toggle shiny sprite"
      >
        <span
          className="shiny-icon"
          style={{
            filter: shinyBtnSelected ? 'none' : 'grayscale(100%)',
            fontSize: '1.7rem',
            display: 'inline-block',
            transition: 'filter 0.2s',
          }}
        >
          ✨
        </span>
      </button>
      <img
        className="card-img-top h-45 pt-2 ps-5 pe-5 mx-auto"
        src={stripGarbageSpriteText(
          shinyBtnSelected
            ? data.sprites?.other?.home?.front_shiny
            : data.sprites?.other?.home?.front_default
        )}
        alt={data.name}
      />
      <div className="card-body">
        <h4 className="card-title text-capitalize text-center">
          {data.name?.replaceAll('-', ' ')}
        </h4>
        <div className="d-flex justify-content-center">
          {data.types?.map((pkmnType: any, idx: number) => (
            <TypeModal key={idx} pkmnType={pkmnType.type} />
          ))}
        </div>
        <div className="d-flex mt-1 flex-wrap justify-content-center">
          <StatsModal data={data} />
          <MovesModal data={data} />
        </div>
      </div>
    </div>
  );
};

export default PokemonCard;
