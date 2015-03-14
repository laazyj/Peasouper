namespace Peasouper.Domain
{
    public class Person : ISupportPartialRetrieval
    {
        public PersonId Id { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public bool? IsAdministrator { get; set; }
        public bool? IsCommunityUser { get; set; }
        public bool? IsVirtualUser { get; set; }
        public bool? ReceiveEmailNotifications { get; set; }
        public string HomepageUrl { get; set; }
        public string Locale { get; set; }
        public string Language { get; set; }
        public string Timezone { get; set; }

        public CaseId? CurrentlyWorkingOn { get; set; }

        public bool Deleted { get; set; }
    }
}
