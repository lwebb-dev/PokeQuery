import React, { useState } from 'react';
import styles from './PokemonCard.module.css';
import classNames from 'classnames';
import TypeModal from './TypeModal';
import MovesModal from './MovesModal';
import StatsModal from './StatsModal';

interface PokemonCardProps {
  data: any;
}

// ...removed inline style, now using CSS module...

const stripGarbageSpriteText = (text?: string) => {
  return text?.replace(
    'https://raw.githubusercontent.com/PokeAPI/sprites/master/https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites',
    'https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites'
  );
};

const PokemonCard: React.FC<PokemonCardProps> = ({ data }) => {

  const [shinyBtnSelected, setShinyBtnSelected] = useState(false);

  // ...removed inline style, now using CSS module...

  return (
    <div className={classNames(styles.cardPkmn, 'card', 'mw-20', 'm-2', 'position-relative')}>
      <button
        className={styles.shinyBtn}
        onClick={() => setShinyBtnSelected(!shinyBtnSelected)}
        aria-label="Toggle shiny sprite"
        // Optionally, you can use a state to toggle a class for hover effect
      >
        <span
          className={styles.shinyIcon}
          style={{ filter: shinyBtnSelected ? 'grayscale(100%) sepia(100%) saturate(500%)' : 'grayscale(100%)' }}
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
