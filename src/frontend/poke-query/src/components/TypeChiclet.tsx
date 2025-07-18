import React, { useState } from 'react';

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
  // Use only Bootstrap classes for margin, padding, border radius, and font size
  const chicletClass = isSmall ? 'px-2 py-1 my-2 mx-2 rounded-pill small d-inline-block text-center' : 'px-3 py-1 my-2 mx-2 rounded-pill d-inline-block text-center';

  const [isHovered, setIsHovered] = useState(false);
  const [isActive, setIsActive] = useState(false);

  const chicletStyleClass : React.CSSProperties = {
    cursor: isStatic ? 'default' : 'pointer',
    backgroundColor: isStatic
      ? typeColor
      : isActive
        ? lightenDarkenColor(typeColor, -50)
        : isHovered
          ? lightenDarkenColor(typeColor, -20)
          : typeColor,
    color: fontColor,
    transition: 'background-color 0.2s',
    outline: 'none',
    boxShadow: 'none',
    userSelect: 'none',
  };

  return (
    <span
      className={`rounded-pill font-bold ${chicletClass}`}
      style={chicletStyleClass}
      tabIndex={-1}
      onFocus={e => { e.currentTarget.style.outline = 'none'; e.currentTarget.style.boxShadow = 'none'; }}
      onBlur={e => { e.currentTarget.style.outline = 'none'; e.currentTarget.style.boxShadow = 'none'; }}
      {...(!isStatic && {
        onMouseEnter: () => setIsHovered(true),
        onMouseLeave: () => { setIsHovered(false); setIsActive(false); },
        onMouseDown: () => setIsActive(true),
        onMouseUp: () => setIsActive(false)
      })}
    >
      {typeName.charAt(0).toUpperCase() + typeName.slice(1)}
    </span>
  );
};

export default TypeChiclet;
