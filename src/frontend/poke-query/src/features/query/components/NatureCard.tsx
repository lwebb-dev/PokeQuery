import React from 'react';
import styles from './NatureCard.module.css';
import classNames from 'classnames';
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
    <div className={classNames(styles.cardNature, 'card', 'mw-20', 'm-2')}>
      <div className="card-body">
        <h2 className="mt-2 card-title text-capitalize text-center">
          {data.name}
        </h2>
        <div className={classNames(styles.statColumn)}>
          {data.increased_stat === null && data.decreased_stat === null ? (
            <h3>Neutral</h3>
          ) : (
            <>
              <h3>{statNames[data.increased_stat?.name]} <i className={classNames('bi', 'bi-arrow-up-circle-fill', styles.statUp)}></i></h3>
              <h3>{statNames[data.decreased_stat?.name]} <i className={classNames('bi', 'bi-arrow-down-circle-fill', styles.statDown)}></i></h3>
            </>
          )}
        </div>
      </div>
    </div>
  );
};

export default NatureCard;
