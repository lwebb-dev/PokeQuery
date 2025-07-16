import React from 'react';

interface StatsModalProps {
  data: any;
}

const StatsModal: React.FC<StatsModalProps> = ({ data }) => {
  // Placeholder: Show base stats
  return (
    <span className="px-2 py-1 rounded bg-blue-200 text-xs font-semibold">
      Stats: {data.stats?.map((stat: any) => `${stat.stat?.name}: ${stat.base_stat}`).join(', ')}
    </span>
  );
};

export default StatsModal;
