/**
 * Utility class for filtering Pokemon search results
 */

/**
 * Extracts species ID from a Pokemon's species URL
 */
const getSpeciesId = (pokemon: any): number | null => {
  if (!pokemon?.species?.url) return null;
  const match = pokemon.species.url.match(/\/(\d+)\/?$/);
  return match ? Number(match[1]) : null;
};

/**
 * Filter rule: Only show default Pikachu form (species ID: 25)
 * Filters out alternate forms like Pikachu-cosplay variants
 */
const filterPikachuForms = (pokemon: any): boolean => {
  const speciesId = getSpeciesId(pokemon);
  if (speciesId === 25) {
    return pokemon.is_default === true;
  }
  return true;
};

/**
 * Filter rule: Hide Mega Evolution forms
 * Filters out Pokemon with "-mega" in their name
 */
const filterMegaForms = (pokemon: any): boolean => {
  return !pokemon.name?.includes('-mega');
};

/**
 * Filter rule: Hide Gigantamax forms
 * Filters out Pokemon with "-gmax" in their name
 */
const filterGmaxForms = (pokemon: any): boolean => {
  return !pokemon.name?.includes('-gmax');
};

/**
 * Array of active filter rules
 * Add or remove filter functions here to enable/disable specific rules
 */
const activeFilters = [
  filterPikachuForms,
  filterMegaForms,
  filterGmaxForms,
  // Add more filter functions here as needed
];

/**
 * Main filter function that applies all active filter rules to Pokemon results
 * Also removes duplicates by Pokemon ID using a Set for O(n) performance
 * @param results - Array of Pokemon objects from API
 * @returns Filtered array of unique Pokemon
 */
export const filterPokemonResults = (results: any[]): any[] => {
  const seen = new Set<number>();

  return results.filter((pokemon) => {
    if (!pokemon) return false;

    // Remove duplicates by ID
    if (seen.has(pokemon.id)) return false;
    seen.add(pokemon.id);

    // Apply all active filter rules
    return activeFilters.every(filterFn => filterFn(pokemon));
  });
};
