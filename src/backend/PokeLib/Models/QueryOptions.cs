namespace PokeLib.Models
{
    public class QueryOptions
    {
        public string Query { get; set; }
        public bool IncludePokemon { get; set; }
        public bool IncludeItems { get; set; }
        public bool IncludeMoves { get; set; }
    }
}
