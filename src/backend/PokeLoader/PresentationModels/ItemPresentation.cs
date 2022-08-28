using System.Text.Json.Serialization;

namespace PokeLoader.PresentationModels
{
    public struct ItemPresentation
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("sprites")]
        public ItemSprites? Sprites { get; set; }

        [JsonPropertyName("effect_entries")]
        public ItemEffectEntries[] EffectEntries { get; set; }
    }

    public struct ItemSprites
    {
        [JsonPropertyName("default")]
        public string Default { get; set; }
    }

    public struct ItemEffectEntries
    {
        [JsonPropertyName("short_effect")]
        public string ShortEffect { get; set; }
    }

}
