import React, { useState, useEffect } from 'react';
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
      // handle error
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    // Load session data on app mount (only once)
    loadSessionData(API_BASE_URI);
  }, []);

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
    <div className="container my-3">
      <h1 className="mb-3 text-center display-4 font-weight-bold">PokeQuery</h1>
      <div className="d-flex justify-content-center mb-4 align-items-center">
        <input
          type="text"
          className="form-control form-control-lg w-auto"
          style={{ minWidth: '300px', maxWidth: '400px' }}
          placeholder="pikachu, leftovers, etc..."
          value={query}
          onChange={e => setQuery(e.target.value)}
          minLength={1}
        />
        <button
          type="submit"
          className="btn btn-primary btn-lg ml-2"
          onClick={handleSearch}
        >
          {isLoading ? (
            <span className="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
          ) : (
            <>Search 🔍</>
          )}
        </button>
      </div>
      <div className="d-flex justify-content-center mb-4 gap-4">
        <label className="d-flex align-items-center mx-2">
          <input type="checkbox" checked={includePokemon} onChange={e => setIncludePokemon(e.target.checked)} />
          <span className="ml-2">Pokemon</span>
        </label>
        <label className="d-flex align-items-center mx-2">
          <input type="checkbox" checked={includeItems} onChange={e => setIncludeItems(e.target.checked)} />
          <span className="ml-2">Items</span>
        </label>
        <label className="d-flex align-items-center mx-2">
          <input type="checkbox" checked={includeMoves} onChange={e => setIncludeMoves(e.target.checked)} />
          <span className="ml-2">Moves</span>
        </label>
        <label className="d-flex align-items-center mx-2">
          <input type="checkbox" checked={includeNatures} onChange={e => setIncludeNatures(e.target.checked)} />
          <span className="ml-2">Natures</span>
        </label>
      </div>
      <div className="d-flex justify-content-center mt-3">
        <div className="row w-100 justify-content-center">
          {isLoading ? (
            <span className="spinner-border text-primary" style={{ fontSize: '2rem' }} role="status"></span>
          ) : (
            <>
              {[...pkmnResults.filter(x => x.is_default),
                ...itemResults.filter(x => !x.name.includes('-candy')),
                ...moveResults,
                ...natureResults].slice(0, 12).map((result, idx) => {
                let CardComponent;
                if (result.types) CardComponent = PokemonCard;
                else if (result.effect_entries) CardComponent = ItemCard;
                else if (result.power !== undefined) CardComponent = MoveCard;
                else if (result.increased_stat) CardComponent = NatureCard;
                else return null;
                return (
                  <div className="col-12 col-sm-6 col-md-4 col-lg-2 d-flex justify-content-center mb-4" key={result.id || idx}>
                    <CardComponent data={result} />
                  </div>
                );
              })}
            </>
          )}
        </div>
      </div>
    </div>
  );
};

export default App;
