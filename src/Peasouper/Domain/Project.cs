namespace Peasouper.Domain
{
    public class Project : ISupportPartialRetrieval
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Person Owner { get; set; }
        public bool IsInbox { get; set; }
        public ProjectType Type { get; set; }
        public string ProjectGroup { get; set; }
        public string PublicSubmitEmail { get; set; }
        public bool Deleted { get; set; }
    }

    public enum ProjectType
    {
        Client = 1,
        Department = 2
    }
}
