using System.Text.Json.Serialization;

namespace PokeLoader.PresentationModels
{
    public struct PokemonSpeciesPresentation
    {
        [JsonPropertyName("evolves_from_species")]
        public PokemonSpecies? EvolvesFromSpecies { get; set; }
    }
}
