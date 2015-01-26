namespace FogBugz.Api.Domain
{
    public class Status : ISupportPartialRetrieval
    {
        public StatusId Id { get; set; }
        public string Name { get; set; }

        public CategoryId? Category { get; set; }
        public bool? WorkDone { get; set; }
        public bool? Resolved { get; set; }
        public bool? Duplicate { get; set; }
        public int? Order { get; set; }
        public bool Deleted { get; set; }
    }
}
