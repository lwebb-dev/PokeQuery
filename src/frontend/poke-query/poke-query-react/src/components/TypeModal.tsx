import React from 'react';
import TypeChiclet from './TypeChiclet';

interface TypeModalProps {
  pkmnType: any;
}

const TypeModal: React.FC<TypeModalProps> = ({ pkmnType }) => {
  // For now, just show the chiclet. Modal logic can be added later if needed.
  return <TypeChiclet typeName={pkmnType?.name || ''} />;
};

export default TypeModal;
