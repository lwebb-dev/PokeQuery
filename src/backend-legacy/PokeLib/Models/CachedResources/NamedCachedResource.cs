namespace PokeLib
{
    public sealed class NamedCachedResource : CachedResourceBase
    {
        public string Name { get; set; }

        public NamedResourceTypes NamedResourceType { get; set; }

        public override string SortIndex => string.Join('_', new[]
{
            ((int)this.NamedResourceType).ToString(),
            this.Id.ToString("00000"),
            this.Name
        });
    }
}
