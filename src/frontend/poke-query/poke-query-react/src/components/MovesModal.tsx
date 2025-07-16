import React from 'react';

interface MovesModalProps {
  data: any;
}

const MovesModal: React.FC<MovesModalProps> = ({ data }) => {
  // Placeholder: Show number of moves
  return (
    <span className="px-2 py-1 rounded bg-green-200 text-xs font-semibold">
      Moves: {data.moves?.length || 0}
    </span>
  );
};

export default MovesModal;
