namespace PokeLib
{
    public class CachedResource
    {
        public string Name { get; set; }
        public ResourceTypes ResourceType { get; set; }
        public string Url { get; set; }
        public string Json { get; set; }
        public int Id => int.Parse(this.Url.Replace("https://pokeapi.co/api/v2/", "").Split('/')[1]);
        public string SortIndex => string.Join('_', new[] 
        { 
            ((int)this.ResourceType).ToString(), 
            this.Id.ToString("00000"),
            this.Name 
        });
    }
}