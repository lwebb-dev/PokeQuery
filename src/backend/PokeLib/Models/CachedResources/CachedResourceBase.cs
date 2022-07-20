namespace PokeLib
{
    public abstract class CachedResourceBase
    {
        public string Url { get; set; }
        public string Json { get; set; }
        public int Id => int.Parse(this.Url.Replace("https://pokeapi.co/api/v2/", "").Split('/')[1]);

        public abstract string SortIndex { get; }
    }
}