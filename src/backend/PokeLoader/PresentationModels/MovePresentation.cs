using System.Text.Json.Serialization;

namespace PokeLoader.PresentationModels
{
    public struct MovePresentation
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public MoveType? Type { get; set; }

        [JsonPropertyName("power")]
        public int? Power { get; set; }

        [JsonPropertyName("accuracy")]
        public int? Accuracy { get; set; }

        [JsonPropertyName("pp")]
        public int? Pp { get; set; }

        [JsonPropertyName("damage_class")]
        public MoveDamageClass? DamageClass { get; set; }

        [JsonPropertyName("effect_entries")]
        public MoveEffectEntries[] EffectEntries { get; set; }

        [JsonPropertyName("effect_chance")]
        public int? EffectChance { get; set; }
    }

    public struct MoveType
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public struct MoveDamageClass
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public struct MoveEffectEntries
    {
        [JsonPropertyName("short_effect")]
        public string ShortEffect { get; set; }
    }

}

