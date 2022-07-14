namespace PokeLib
{
    public class CachedResource
    {
        public string Name { get; set; }
        public ResourceTypes ResourceType { get; set; }
        public string Url { get; set; }
        public string Json { get; set; }
        public string SortIndex => string.Join('_', new[] 
        { 
            ((int)this.ResourceType).ToString(), 
            this.Name 
        });
    }
}