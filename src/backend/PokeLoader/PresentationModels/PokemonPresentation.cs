using System.Text.Json.Serialization;

namespace PokeLoader.PresentationModels
{
    public struct PokemonPresentation
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("is_default")]
        public bool IsDefault { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("sprites")]
        public PokemonSprites? Sprites { get; set; }

        [JsonPropertyName("types")]
        public PokemonType[] Types { get; set; }

        [JsonPropertyName("stats")]
        public PokemonStat[] Stats { get; set; }

        [JsonPropertyName("moves")]
        public PokemonMove[] Moves { get; set; }

        [JsonPropertyName("species")]
        public PokemonSpecies? Species { get; set; }
    }

    public struct PokemonSpecies
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public struct PokemonEvolutionSpecies
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public struct PokemonMove
    {
        [JsonPropertyName("version_group_details")]
        public MoveVersionGroupDetails[] VersionGroupDetails { get; set; }

        [JsonPropertyName("move")]
        public PokemonMoveDetails? Move { get; set; }
    }

    public struct PokemonMoveDetails
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public struct MoveVersionGroupDetails
    {
        [JsonPropertyName("level_learned_at")]
        public int LevelLearnedAt { get; set; }

        [JsonPropertyName("version_group")]
        public MoveVersionGroup? VersionGroup { get; set; }

        [JsonPropertyName("move_learn_method")]
        public MoveLearnMethod? MoveLearnMethod { get; set; }
    }

    public struct MoveVersionGroup
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public struct MoveLearnMethod
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public struct PokemonStat
    {
        [JsonPropertyName("base_stat")]
        public int BaseStat { get; set; }
        [JsonPropertyName("stat")]
        public PokemonStatDetails? Stat { get; set; }
    }

    public struct PokemonStatDetails
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public struct PokemonType
    {
        [JsonPropertyName("type")]
        public PokeLoader.PresentationModels.Type? Type { get; set; }
    }

    public struct Type
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public struct PokemonSprites
    {
        [JsonPropertyName("other")]
        public OtherSprites? Other { get; set; }
    }

    public struct OtherSprites
    {
        [JsonPropertyName("home")]
        public HomeSprites? Home { get; set; }
    }

    public struct HomeSprites
    {
        [JsonPropertyName("front_default")]
        public string FrontDefault { get; set; }

        [JsonPropertyName("front_shiny")]
        public string FrontShiny { get; set; }

    }
}
