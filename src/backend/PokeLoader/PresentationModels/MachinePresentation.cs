using System.Text.Json.Serialization;

namespace PokeLoader.PresentationModels
{
    public struct MachinePresentation
    {
        [JsonPropertyName("move")]
        public MachineMove? Move { get; set; }

        [JsonPropertyName("item")]
        public MachineItem? Item { get; set; }

        [JsonPropertyName("version_group")]
        public MachineVersionGroup? VersionGroup { get; set; }
    }

    public struct MachineVersionGroup
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public struct MachineMove
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public struct MachineItem
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
