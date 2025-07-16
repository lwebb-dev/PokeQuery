import React from 'react';

interface TypeChicletProps {
  typeName: string;
  isStatic?: boolean;
  isSmall?: boolean;
}

const typeColors: Record<string, string> = {
  grass: '#78C850',
  fire: '#F08030',
  water: '#6890F0',
  bug: '#A8B820',
  normal: '#A8A878',
  flying: '#a396e0',
  poison: '#A040A0',
  electric: '#F8D030',
  ground: '#E0C068',
  fairy: '#EE99AC',
  fighting: '#C03028',
  psychic: '#F85888',
  rock: '#B8A038',
  ghost: '#705898',
  ice: '#98D8D8',
  dragon: '#7038F8',
  steel: '#919191',
  dark: '#4d4646',
};

const blackTypes = ['electric', 'fairy', 'ice', 'ground'];

function lightenDarkenColor(col: string, amt: number) {
  let usePound = false;
  if (col[0] === '#') {
    col = col.slice(1);
    usePound = true;
  }
  let num = parseInt(col, 16);
  let r = (num >> 16) + amt;
  let b = ((num >> 8) & 0x00FF) + amt;
  let g = (num & 0x0000FF) + amt;
  let newColor = (g | (b << 8) | (r << 16)).toString(16);
  return (usePound ? '#' : '') + newColor.padStart(6, '0');
}

const TypeChiclet: React.FC<TypeChicletProps> = ({ typeName, isStatic, isSmall }) => {
  const typeColor = typeColors[typeName] || '#A8A878';
  const fontColor = blackTypes.includes(typeName) ? 'black' : 'white';
  const chicletClass = isSmall ? 'px-2 py-1 text-xs' : 'px-3 py-1 text-sm';

  return (
    <span
      className={`rounded font-bold ${chicletClass}`}
      style={{ backgroundColor: typeColor, color: fontColor }}
    >
      {typeName}
    </span>
  );
};

export default TypeChiclet;
