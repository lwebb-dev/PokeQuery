
import React, { useState, useEffect } from 'react';
import styles from './App.module.css';
import classNames from 'classnames';
import { loadSessionData } from './sessionData';
import PokemonCard from './components/PokemonCard';
import ItemCard from './components/ItemCard';
import MoveCard from './components/MoveCard';
import NatureCard from './components/NatureCard';

const API_BASE_URI = import.meta.env.VITE_API_BASE_URI || '';

const App: React.FC = () => {

  const [query, setQuery] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const [pkmnResults, setPkmnResults] = useState<any[]>([]);
  const [itemResults, setItemResults] = useState<any[]>([]);
  const [moveResults, setMoveResults] = useState<any[]>([]);
  const [natureResults, setNatureResults] = useState<any[]>([]);

  const [includePokemon, setIncludePokemon] = useState(true);
  const [includeItems, setIncludeItems] = useState(false);
  const [includeMoves, setIncludeMoves] = useState(false);
  const [includeNatures, setIncludeNatures] = useState(false);

  const handleQuery = async (prefix: string, flag: boolean) => {

    if (!flag) return [];

    const res = await fetch(`${API_BASE_URI}/search/${prefix}/${query}`, {
      method: 'GET',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
    });
    if (!res.ok) throw new Error('API FAILED TO RETURN 200 OK ON /search');
    const data = await res.json();
    return data.map((x: string) => JSON.parse(x));
  };

  const handleSearch = async () => {

    if (!query) return;

    setIsLoading(true);
    setPkmnResults([]);
    setItemResults([]);
    setMoveResults([]);
    setNatureResults([]);

    try {
      const [pkmn, items, moves, natures] = await Promise.all([
        handleQuery('pokemon', includePokemon),
        handleQuery('item', includeItems),
        handleQuery('move', includeMoves),
        handleQuery('nature', includeNatures),
      ]);

      setPkmnResults(pkmn.sort((a: any, b: any) => a.id - b.id));
      setItemResults(items);
      setMoveResults(moves);
      setNatureResults(natures);
    } catch (e) {
      console.error('Error fetching data:', e);
      alert('An error occurred while fetching data.');
    } finally {
      setIsLoading(false);
    }
  };

  // Load session data on app mount (only once)
  useEffect(() => {
    loadSessionData(API_BASE_URI);
  }, []);

  // Map Enter key to Search button click
  useEffect(() => {

    const handleKeyDown = (event: KeyboardEvent) => {
      if (event.key === 'Enter') {
        handleSearch();
      }
    };
    
    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, [query, includePokemon, includeItems, includeMoves, includeNatures]);

  return (
    <div className={classNames(styles.appWrapper, 'container-fluid', 'my-3')}>
      <h1 className="row mb-3 justify-content-center">PokeQuery</h1>
      <div className="row d-flex justify-content-center mb-4 align-items-center">
        <div className={classNames(styles.searchInput, 'col', 'col-lg-3')}>
          <input
            type="text"
            className="form-control form-control-lg"
            placeholder="pikachu, leftovers, etc..."
            value={query}
            onChange={e => setQuery(e.target.value)}
            minLength={1}
          />
        </div>
        <div className="col-auto">
          <button
            type="submit"
            className={classNames(styles.btnSearch, 'btn', 'btn-primary', 'btn-lg')}
            onClick={handleSearch}
          >
            {isLoading ? (
              <span className="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
            ) : (
              <>Search 🔎</>
            )}
          </button>
        </div>
      </div>
      <div className="d-flex justify-content-center mb-4">
        <div className="form-check form-check-inline">
          <input className="form-check-input" type="checkbox" checked={includePokemon} onChange={e => setIncludePokemon(e.target.checked)} id="cbPkmn" />
          <label className="form-check-label" htmlFor="cbPkmn">Pokemon</label>
        </div>
        <div className="form-check form-check-inline">
          <input className="form-check-input" type="checkbox" checked={includeItems} onChange={e => setIncludeItems(e.target.checked)} id="cbItems" />
          <label className="form-check-label" htmlFor="cbItems">Items</label>
        </div>
        <div className="form-check form-check-inline">
          <input className="form-check-input" type="checkbox" checked={includeMoves} onChange={e => setIncludeMoves(e.target.checked)} id="cbMoves" />
          <label className="form-check-label" htmlFor="cbMoves">Moves</label>
        </div>
        <div className="form-check form-check-inline">
          <input className="form-check-input" type="checkbox" checked={includeNatures} onChange={e => setIncludeNatures(e.target.checked)} id="cbNatures" />
          <label className="form-check-label" htmlFor="cbNatures">Natures</label>
        </div>
      </div>
      <div className="container mt-3 d-flex flex-wrap justify-content-center">
        {isLoading ? (
          <span className={classNames(styles.cardContainerSpinner, 'spinner-border', 'text-primary')} role="status"></span>
        ) : (
          <>
            {[...pkmnResults.filter(x => x.is_default),
              ...itemResults.filter(x => !x.name.includes('-candy')),
              ...moveResults,
              ...natureResults].slice(0, 12).map((result, idx) => {
              let CardComponent;
              if (result.types) CardComponent = PokemonCard;
              else if (result.power !== undefined || result.accuracy !== undefined || result.pp !== undefined) CardComponent = MoveCard;
              else if (result.increased_stat !== undefined || result.decreased_stat !== undefined) CardComponent = NatureCard;
              else if (result.effect_entries) CardComponent = ItemCard;
              else return null;
              return (
                <CardComponent data={result} key={result.id || idx} />
              );
            })}
          </>
        )}
      </div>
    </div>
  );

}
export default App;
