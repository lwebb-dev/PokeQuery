import React, { useState, useEffect } from 'react';
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
    const handleKeyDown = (event: KeyboardEvent) => {
      if (event.key === 'Enter') {
        handleSearch();
      }
    };
    window.addEventListener('keydown', handleKeyDown);
    return () => window.removeEventListener('keydown', handleKeyDown);
  }, [query, includePokemon, includeItems, includeMoves, includeNatures]);

  return (
    <div className="container mx-auto my-3">
      <h1 className="mb-3 text-center text-3xl font-bold">PokeQuery</h1>
      <div className="flex justify-center mb-4">
        <input
          type="text"
          className="form-input text-lg px-4 py-2 border rounded-lg"
          placeholder="pikachu, leftovers, etc..."
          value={query}
          onChange={e => setQuery(e.target.value)}
          minLength={1}
        />
        <button
          type="submit"
          className="btn btn-primary text-lg ml-2 px-4 py-2 rounded-lg bg-blue-500 text-white"
          onClick={handleSearch}
        >
          {isLoading ? (
            <span className="animate-spin">🔄</span>
          ) : (
            <>Search 🔍</>
          )}
        </button>
      </div>
      <div className="flex justify-center mb-4 gap-4">
        <label className="flex items-center">
          <input type="checkbox" checked={includePokemon} onChange={e => setIncludePokemon(e.target.checked)} />
          <span className="ml-2">Pokemon</span>
        </label>
        <label className="flex items-center">
          <input type="checkbox" checked={includeItems} onChange={e => setIncludeItems(e.target.checked)} />
          <span className="ml-2">Items</span>
        </label>
        <label className="flex items-center">
          <input type="checkbox" checked={includeMoves} onChange={e => setIncludeMoves(e.target.checked)} />
          <span className="ml-2">Moves</span>
        </label>
        <label className="flex items-center">
          <input type="checkbox" checked={includeNatures} onChange={e => setIncludeNatures(e.target.checked)} />
          <span className="ml-2">Natures</span>
        </label>
      </div>
      <div className="flex flex-wrap justify-center mt-3 gap-4">
        {isLoading ? (
          <span className="animate-spin text-2xl">🔄 Loading...</span>
        ) : (
          <>
            {pkmnResults.filter(x => x.is_default).map(pkmn => (
              <PokemonCard key={pkmn.id} data={pkmn} />
            ))}
            {itemResults.filter(x => !x.name.includes('-candy')).map(item => (
              <ItemCard key={item.id} data={item} />
            ))}
            {moveResults.map(move => (
              <MoveCard key={move.id} data={move} />
            ))}
            {natureResults.map(nature => (
              <NatureCard key={nature.id} data={nature} />
            ))}
          </>
        )}
      </div>
    </div>
  );
};

export default App;
