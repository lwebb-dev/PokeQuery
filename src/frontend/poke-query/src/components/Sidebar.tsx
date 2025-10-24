import React from 'react';
import { Link, useLocation } from 'react-router-dom';

const Sidebar: React.FC = () => {
  const location = useLocation();

  return (
    <div className="d-flex flex-column flex-shrink-0 p-3 text-white bg-dark" style={{ width: '280px' }}>
      <Link to="/" className="d-flex align-items-center mb-3 mb-md-0 me-md-auto text-white text-decoration-none">
        <span className="fs-4">PokeQuery</span>
      </Link>
      <hr />
      <ul className="nav nav-pills flex-column mb-auto">
        <li className="nav-item">
          <Link
            to="/query"
            className={`nav-link ${location.pathname === '/query' ? 'active' : 'text-white'}`}
            aria-current={location.pathname === '/query' ? 'page' : undefined}
          >
            <i className="bi bi-search me-2"></i>
            Query
          </Link>
        </li>
        <li>
          <Link
            to="/team-builder"
            className={`nav-link ${location.pathname === '/team-builder' ? 'active' : 'text-white'}`}
            aria-current={location.pathname === '/team-builder' ? 'page' : undefined}
          >
            <i className="bi bi-people-fill me-2"></i>
            Team Builder
          </Link>
        </li>
      </ul>
    </div>
  );
};

export default Sidebar;
