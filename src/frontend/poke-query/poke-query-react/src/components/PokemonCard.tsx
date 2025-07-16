import React, { useState } from 'react';
import TypeModal from './TypeModal';
import MovesModal from './MovesModal';
import StatsModal from './StatsModal';

interface PokemonCardProps {
  data: any;
}

const stripGarbageSpriteText = (text?: string) => {
  return text?.replace(
    'https://raw.githubusercontent.com/PokeAPI/sprites/master/https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites',
    'https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites'
  );
};

const PokemonCard: React.FC<PokemonCardProps> = ({ data }) => {
  const [shinyBtnSelected, setShinyBtnSelected] = useState(false);

  return (
    <div className="card mw-20 m-2 border-4 border-[#fc8686] w-60 h-[21rem] relative">
      <button
        className={`absolute top-2 left-2 shiny-btn ${shinyBtnSelected ? 'shiny-selected' : ''}`}
        onClick={() => setShinyBtnSelected(!shinyBtnSelected)}
      >
        ✨
      </button>
      <img
        className="card-img-top h-[45%] pt-2 ps-5 pe-5 mx-auto"
        src={stripGarbageSpriteText(
          shinyBtnSelected
            ? data.sprites?.other?.home?.front_shiny
            : data.sprites?.other?.home?.front_default
        )}
        alt={data.name}
      />
      <div className="card-body p-4">
        <h4 className="card-title text-capitalize text-center text-lg font-bold">
          {data.name?.replaceAll('-', ' ')}
        </h4>
        <div className="flex justify-center gap-2 my-2">
          {data.types?.map((pkmnType: any, idx: number) => (
            <TypeModal key={idx} pkmnType={pkmnType.type} />
          ))}
        </div>
        <div className="flex mt-1 flex-wrap justify-center gap-2">
          <StatsModal data={data} />
          <MovesModal data={data} />
        </div>
      </div>
    </div>
  );
};

export default PokemonCard;
