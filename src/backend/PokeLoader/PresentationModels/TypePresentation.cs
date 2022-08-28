using System.Text.Json.Serialization;

namespace PokeLoader.PresentationModels
{
    public struct TypePresentation
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("damage_relations")]
        public TypeDamageRelations? DamageRelations { get; set; }
    }

    public struct TypeDamageRelations
    {
        [JsonPropertyName("double_damage_to")]
        public DamageRelationType[] DoubleDamageTo { get; set; } 

        [JsonPropertyName("half_damage_to")]
        public DamageRelationType[] HalfDamageTo { get; set; }

        [JsonPropertyName("no_damage_to")]
        public DamageRelationType[] NoDamageTo { get; set; }

        [JsonPropertyName("double_damage_from")]
        public DamageRelationType[] DoubleDamageFrom { get; set; }

        [JsonPropertyName("half_damage_from")]
        public DamageRelationType[] HalfDamageFrom { get; set; }

        [JsonPropertyName("no_damage_from")]
        public DamageRelationType[] NoDamageFrom { get; set; }

    }

    public struct DamageRelationType
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
