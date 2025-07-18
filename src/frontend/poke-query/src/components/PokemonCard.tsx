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
    width: "240px",
    height: "326px"
  };

const stripGarbageSpriteText = (text?: string) => {
  return text?.replace(
    'https://raw.githubusercontent.com/PokeAPI/sprites/master/https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites',
    'https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites'
  );
};

const PokemonCard: React.FC<PokemonCardProps> = ({ data }) => {

  const [shinyBtnSelected, setShinyBtnSelected] = useState(false);

  const shinyBtnStyle : React.CSSProperties = {
    textAlign: 'center',
    height: '40px',
    width: '40px',
    borderRadius: '25%',
    borderColor: 'rgba(0, 0, 0, 0.0)',
    backgroundColor: 'rgba(0, 0, 0, 0.0)',
    cursor: 'pointer',
    zIndex: 5,
    top: '0.5rem',
    right: '0.5rem',
    position: 'absolute',
    border: 'none',
    padding: 0,
    outline: 'none',
    boxShadow: 'none',
  };


  const shinyIconStyle = {
    filter: shinyBtnSelected ? 'grayscale(100%) sepia(100%) saturate(500%)' : 'grayscale(100%)',
    fontSize: '1.2rem',
    display: 'inline-block',
    transition: 'filter 0.2s',
  };

  return (
    <div style={cardPkmnStyle} className="card mw-20 m-2 position-relative">
      <button
        style={shinyBtnStyle}
        onClick={() => setShinyBtnSelected(!shinyBtnSelected)}
        aria-label="Toggle shiny sprite"
        onMouseOver={e => (e.currentTarget.style.backgroundColor = shinyBtnSelected ? 'rgba(239, 239, 153, 0)' : 'rgba(0, 0, 0, 0.0)')}
        onMouseOut={e => (e.currentTarget.style.backgroundColor = 'rgba(0, 0, 0, 0.0)')}
      >
        <span
          style={shinyIconStyle}
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
