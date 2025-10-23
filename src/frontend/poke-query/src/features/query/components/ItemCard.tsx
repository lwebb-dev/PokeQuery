import React from 'react';
import styles from './ItemCard.module.css';
import classNames from 'classnames';

interface ItemCardProps {
  data: any;
}

const ItemCard: React.FC<ItemCardProps> = ({ data }) => {
  // Fix sprite URL if needed
  const spriteUrl = data.sprites?.default?.replace(
    'https://raw.githubusercontent.com/PokeAPI/sprites/master/https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites',
    'https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites'
  );

  return (
    <div className={classNames(styles.cardItem, 'card', 'mw-20', 'm-2')}>
      <img
        className="card-img-top h-35 pt-2 ps-5 pe-5 mx-auto"
        src={spriteUrl}
        alt={data.name}
      />
      <div className="card-body">
        <h4 className="card-title text-capitalize text-center">
          {data.name?.replaceAll('-', ' ')}
        </h4>
        {data.effect_entries && data.effect_entries[0] && (
          <p className="card-text text-wrap">
            {data.effect_entries[0].short_effect}
          </p>
        )}
      </div>
    </div>
  );
};

export default ItemCard;
