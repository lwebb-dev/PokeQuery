import React from 'react';

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
    <div className="card mw-20 m-2 border-4 border-[#ecf296] w-60 h-[21rem]">
      <img
        className="card-img-top h-[35%] pt-2 ps-5 pe-5 mx-auto"
        src={spriteUrl}
        alt={data.name}
      />
      <div className="card-body p-4">
        <h4 className="card-title text-capitalize text-center text-lg font-bold">
          {data.name?.replaceAll('-', ' ')}
        </h4>
        {data.effect_entries && data.effect_entries[0] && (
          <p className="card-text text-wrap text-sm mt-2">
            {data.effect_entries[0].short_effect}
          </p>
        )}
      </div>
    </div>
  );
};

export default ItemCard;
