namespace FogBugz.Api
{
    public static class CaseColumns
    {
        public static readonly string[] Defaults = new[]
	    {
	        Title,
            "sProject",
            "sArea",
            "sFixFor",
            "sAssignedTo",
            "sCategory",
            "sPriority",
            "sStatus"
	    };

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
