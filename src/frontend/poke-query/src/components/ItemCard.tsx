import React from 'react';

interface ItemCardProps {
  data: any;
}

const cardItemStyle = {
  borderColor: "#ecf296",
  borderWidth: "0.35em",
  fontSize: "0.85rem",
  width: "240px",
  height: "326px"
};

const ItemCard: React.FC<ItemCardProps> = ({ data }) => {
  // Fix sprite URL if needed
  const spriteUrl = data.sprites?.default?.replace(
    'https://raw.githubusercontent.com/PokeAPI/sprites/master/https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites',
    'https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites'
  );

  return (
    <div style={cardItemStyle} className="card mw-20 m-2">
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
