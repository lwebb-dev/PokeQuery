
import React from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import Query from './features/query/Query';

const App: React.FC = () => {
  return (
    <Routes>
      <Route path="/" element={<Navigate to="/query" replace />} />
      <Route path="/query" element={<Query />} />
    </Routes>
  );
}

export default App;
