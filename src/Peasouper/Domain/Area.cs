namespace Peasouper.Domain
{
    public class Area : ISupportPartialRetrieval
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Project Project { get; set; }
        public Person Owner { get; set; }
        public bool Deleted { get; set; }
    }
}
