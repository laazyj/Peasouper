namespace FogBugz.Api.Domain
{
    public class Category : ISupportPartialRetrieval
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Plural { get; set; }
        public StatusId DefaultStatus { get; set; }
        public StatusId DefaultActiveStatus { get; set; }
        public bool IsScheduledItem { get; set; }
        public int DisplayOrder { get; set; }
        public IconType IconType { get; set; }
        public AttachmentIconId? CustomIcon { get; set; }
        public bool Deleted { get; set; }
    }

    public enum IconType
    {
    }
}
