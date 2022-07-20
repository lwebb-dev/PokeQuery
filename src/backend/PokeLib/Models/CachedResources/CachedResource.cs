namespace PokeLib
{
    public sealed class CachedResource : CachedResourceBase
    {
        public ResourceTypes ResourceType { get; set; }

        public override string SortIndex => string.Join('_', new[]
{
            ((int)this.ResourceType).ToString(),
            this.Id.ToString("00000")
        });
    }
}
