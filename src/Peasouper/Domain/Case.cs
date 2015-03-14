using System;

namespace Peasouper.Domain
{
    // TODO: Protection on read-only fields.
    public class Case
    {
        /// <summary>
        /// ixBug
        /// </summary>
        public CaseId Id { get; set; }
        /// <summary>
        /// ixBugParent
        /// </summary>
        public CaseId? Parent { get; set; }
        public CaseId[] Children { get; set; }
        public CaseId[] Duplicates { get; set; }
        public CaseId? Original { get; set; }
        public CaseId[] Related { get; set; }

        public bool IsOpen { get; set; }

        public string Title { get; set; }
        public string OriginalTitle { get; set; }
        public string LatestSummary { get; set; }

        public Project Project { get; set; }
        public Area Area { get; set; }

        public DateTime? OpenedDate { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }

        public Person AssignedTo { get; set; }
        public PersonId OpenedBy { get; set; }
        public PersonId? ResolvedBy { get; set; }
        public PersonId? ClosedBy { get; set; }
        public PersonId? LastEditedBy { get; set; }

        public Category Category { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public Milestone FixFor { get; set; }
        public DateTime? DueDate { get; set; }

        public string[] Tags { get; set; }

        public string Version { get; set; }
        public string Computer { get; set; }
        public string ReleaseNotes { get; set; }

        public decimal? EstimateHoursOriginal { get; set; }
        public decimal? EstimateHoursCurrent { get; set; }
        public decimal? ElapsedHours { get; set; }

        public string CustomerEmail { get; set; }
        public MailboxId? Mailbox { get; set; }

        public string ScoutDescription { get; set; }
        public string ScoutMessage { get; set; }
        public bool ScoutStopReporting { get; set; }
        public DateTime? ScoutLastOccurrence { get; set; }
        public int Occurrences { get; set; }

        /// <summary>
        /// Current version of the case.
        /// </summary>
        public EventId? LatestEvent { get; set; }
        public EventId? LatestTextEvent { get; set; }

        public bool Replied { get; set; }
        public bool Forwarded { get; set; }
        /// <summary>
        /// id for customer to view bug (bug number + 8 letters e.g. 4003_XFLFFFCS)
        /// </summary>
        public string Ticket { get; set; }
        public DiscussionId? DiscussionTopic { get; set; }

        public EventId? LastViewedEvent { get; set; }
        public DateTime? LastViewedDate { get; set; }
        public bool Subscribed { get; set; }
    }
}
