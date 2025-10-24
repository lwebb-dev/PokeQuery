
import React from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import Sidebar from './components/Sidebar';
import Query from './features/query/Query';

const App: React.FC = () => {
  return (
    <div className="d-flex" style={{ minHeight: '100vh' }}>
      <Sidebar />
      <div className="flex-grow-1 my-3">
        <Routes>
          <Route path="/" element={<Navigate to="/query" replace />} />
          <Route path="/query" element={<Query />} />
          <Route path="/team-builder" element={<div className="p-4"><h2>Team Builder</h2><p>Coming soon...</p></div>} />
        </Routes>
      </div>
    </div>
  );
}

export default App;
