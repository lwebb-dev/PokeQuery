import React from 'react';
import TypeModal from './TypeModal';
import styles from './MoveCard.module.css';
import classNames from 'classnames';

interface MoveCardProps {
  data: any;
}

// ...removed inline style, now using CSS module...


const handleNull = (value: any) => {
  if (value === null || value < 5) return '--';
  return value;
};

const interpolateEffectChance = (effect: string, chance: any) => {
  return effect.replaceAll('$effect_chance%', `${chance}%`);
};

const MoveCard: React.FC<MoveCardProps> = ({ data }) => {
  return (
    <div className={classNames(styles.cardMove, 'card', 'mw-20', 'm-2')}>
      <div className="card-body">
        <h4 className="card-title text-capitalize text-center">
          {data.name?.replaceAll('-', ' ')}
        </h4>
        <div className="d-flex justify-content-center">
          <TypeModal pkmnType={data.type} />
        </div>
        <div className={classNames(styles.statsRow, 'd-flex', 'flex-wrap', 'justify-content-center', 'my-2')}>
          <p className={styles.statsText}><strong>Power:</strong> {handleNull(data.power)}</p>
          <p className={styles.statsText}><strong>Accuracy:</strong> {handleNull(data.accuracy)}</p>
          <p className={styles.statsText}><strong>PP:</strong> {handleNull(data.pp)}</p>
          <p className={classNames('text-capitalize', styles.statsText)}><strong>Type:</strong> {handleNull(data.damage_class?.name)}</p>
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
