using System.Text.Json.Serialization;

namespace PokeLoader.PresentationModels
{
    public struct VersionGroupPresentation
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
