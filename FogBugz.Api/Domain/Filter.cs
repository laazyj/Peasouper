namespace FogBugz.Api.Domain
{
    public class Filter
    {
        public FilterId Id { get; set; }
        public string Name { get; set; }
        public FilterType Type { get; set; }
    }

    public enum FilterType
    {
        BuiltIn,
        Saved,
        Shared
    }
}
