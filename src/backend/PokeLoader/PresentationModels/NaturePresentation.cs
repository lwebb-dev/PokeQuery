using System.Text.Json.Serialization;

namespace PokeLoader.PresentationModels
{
    public struct NaturePresentation
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("increased_stat")]
        public NatureStat? IncreasedStat { get; set; }

        [JsonPropertyName("decreased_stat")]
        public NatureStat? DecreasedStat { get; set; }
    }

    public struct NatureStat
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
