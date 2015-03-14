namespace Peasouper
{
    public static class CaseColumns
    {
        public static readonly string[] Defaults =
        {
            Title,
            ProjectId,
            Project,
            AreaId,
            Area,
            FixFor,
            AssignedTo,
            CategoryId,
            Category,
            Priority,
            Status
        };

        public const string Area = "sArea";
        public const string AssignedToPersonId = "ixPersonAssignedTo";
        public const string AssignedTo = "sPersonAssignedTo";
        public const string AssignedToEmail = "sEmailAssignedTo";
        public const string OpenedByPersonId = "ixPersonOpenedBy";
        public const string ResolvedByPersonId = "ixPersonResolvedBy";
        public const string ClosedByPersonId = "ixPersonClosedBy";
        public const string LastEditedByPersonId = "ixPersonLastEditedBy";
        public const string StatusId = "ixStatus";
        public const string Status = "sStatus";
        public const string PriorityId = "ixPriority";
        public const string Priority = "sPriority";
        public const string FixForMilestoneId = "ixFixFor";
        public const string FixFor = "sFixFor";
        public const string FixForDate = "dtFixFor";
        public const string Version = "sVersion";
        public const string Computer = "sComputer";
        public const string EstimateHoursOriginal = "hrsOrigEst";
        public const string EstimateHoursCurrent = "hrsCurrEst";
        public const string ElapsedHours = "hrsElapsed";
        public const string Occurrences = "c";
        public const string CustomerEmail = "sCustomerEmail";
        public const string MailboxId = "ixMailbox";
        public const string CategoryId = "ixCategory";
        public const string Category = "sCategory";
        public const string OpenedDate = "dtOpened";
        public const string ResolvedDate = "dtResolved";
        public const string ClosedDate = "dtClosed";
        public const string LatestEventId = "ixBugEventLatest";
        public const string LastUpdatedDate = "dtLastUpdated";
        public const string Replied = "fReplied";
        public const string Forwarded = "fForwarded";
        public const string Ticket = "sTicket";
        public const string DiscussionTopicId = "ixDiscussTopic";
        public const string DueDate = "dtDue";
        public const string ReleaseNotes = "sReleaseNotes";
        public const string LastViewedEventId = "ixBugEventLastView";
        public const string LastViewedDate = "dtLastView";
        public const string ScoutDescription = "sScoutDescription";
        public const string ScoutMessage = "sScoutMessage";
        public const string ScoutStopReporting = "fScoutStopReporting";
        public const string ScoutLastOccurrence = "dtLastOccurrence";
        public const string Subscribed = "fSubscribed";
        public const string Tags = "tags";
        public const string ProjectId = "ixProject";
        public const string Project = "sProject";
        public const string Children = "ixBugChildren";
        public const string Duplicates = "ixBugDuplicates";
        public const string Original = "ixBugOriginal";
        public const string Related = "ixRelatedBugs";
        public const string IsOpen = "fOpen";
        public const string Title = "sTitle";
        public const string OriginalTitle = "sOriginalTitle";
        public const string LatestSummary = "sLatestTextSummary";
        public const string LatestTextEvent = "ixBugEventLatestText";
        public const string AreaId = "ixArea";
        public const string Parent = "ixBugParent";
    }
}
