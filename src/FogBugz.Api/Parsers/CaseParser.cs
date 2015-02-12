using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FogBugz.Api.Domain;

namespace FogBugz.Api.Parsers
{
    public class CaseParser
    {
        public void Parse(XElement response)
        {
            var cases = response.Element("cases").Elements("case");
            Cases = cases.Select(parseCase).ToArray();
        }

        Case parseCase(XElement element)
        {
            var result = new Case
                {
                    Id = (CaseId)getInt(element.Element("ixBug")),
                    Parent = CaseId.FromInt(getInt(element.Element("ixBugParent"))),
                    Children = getCaseIds(element.Element("ixChildren")),
                    Duplicates = getCaseIds(element.Element("ixBugDuplicates")),
                    Original = CaseId.FromInt(getInt(element.Element("ixBugOriginal"))),
                    Related = getCaseIds(element.Element("ixRelatedBugs")),
                    Tags = getStrings(element.Element("tags").Elements("tag")),
                    IsOpen = getBool(element.Element("fOpen")),
                    Title = getString(element.Element("sTitle")),
                    OriginalTitle = getString(element.Element("sOriginalTitle")),
                    LatestSummary = getString(element.Element("sLatestTextSummary")),
                    LatestTextEvent = EventId.FromInt(getInt(element.Element("ixBugEventLatestText"))),
                    Project = getProject(element),
                    Area = new Area
                        {
                            Id = element.Element("ixArea"),
                            Name = element.Element("sArea")
                        },
                    AssignedTo = new Person
                        {
                            Id = element.Element("ixPersonAssignedTo"),
                            FullName = element.Element("sPersonAssignedTo"),
                            Email = element.Element("sEmailAssignedTo")
                        },
                    OpenedBy = PersonId.FromInt(getInt(element.Element("ixPersonOpenedBy"))),
                    ResolvedBy = PersonId.FromInt(getInt(element.Element("ixPersonResolvedBy"))),
                    ClosedBy = PersonId.FromInt(getInt(element.Element("ixPersonClosedBy"))),
                    LastEditedBy = PersonId.FromInt(getInt(element.Element("ixPersonLastEditedBy"))),
                    Status = new Status
                    {
                        Id = element.Element("ixStatus"),
                        Name = element.Element("sStatus")
                    },
                    Priority = new Priority
                        {
                            Id = element.Element("ixPriority"),
                            Name = element.Element("sPriority")
                        },
                    FixFor = new Milestone
                        {
                            Id = element.Element("ixFixFor"),
                            Name = element.Element("sFixFor"),
                            Date = element.Element("dtFixFor")
                        },
                    Version = getString(element.Element("sVersion")),
                    Computer = getString(element.Element("sComputer")),
                    EstimateHoursOriginal = getDecimal(element.Element("hrsOrigEst")),
                    EstimateHoursCurrent = getDecimal(element.Element("hrsCurrEst")),
                    ElapsedHours = getDecimal(element.Element("hrsElapsed")),
                    Occurrences = getInt(element.Element("c")) + 1,
                    CustomerEmail = getString(element.Element("sCustomerEmail")),
                    Mailbox = MailboxId.FromInt(getInt(element.Element("ixMailbox"))),
                    Category = new Category
                        {
                            Id = element.Element("ixCategory"),
                            Name = element.Element("sCategory")
                        },
                    OpenedDate = getDate(element.Element("dtOpened")),
                    ResolvedDate = getDate(element.Element("dtResolved")),
                    ClosedDate = getDate(element.Element("dtClosed")),
                    LatestEvent = EventId.FromInt(getInt(element.Element("ixBugEventLatest"))),
                    LastUpdatedDate = getDate(element.Element("dtLastUpdated")),
                    Replied = getBool(element.Element("fReplied")),
                    Forwarded = getBool(element.Element("fForwarded")),
                    Ticket = getString(element.Element("sTicket")),
                    DiscussionTopic = element.Element("ixDiscussTopic"),
                    DueDate = getDate(element.Element("dtDue")),
                    ReleaseNotes = getString(element.Element("sReleaseNotes")),
                    LastViewedEvent = EventId.FromInt(getInt(element.Element("ixBugEventLastView"))),
                    LastViewedDate = getDate(element.Element("dtLastView")),
                    ScoutDescription = getString(element.Element("sScoutDescription")),
                    ScoutMessage = getString(element.Element("sScountMessage")),
                    ScoutStopReporting = getBool(element.Element("fScoutStopReporting")),
                    ScoutLastOccurrence = getDate(element.Element("dtLastOccurrence")),
                    Subscribed = getBool(element.Element("fSubscribed"))
                };
            var operations = element.Attribute("operations");
        }

        private static DateTime? getDate(XElement element)
        {
            return element == null ? (DateTime?) null : DateTime.Parse(element.Value);
        }

        private static decimal? getDecimal(XElement element)
        {
            return element == null ? (decimal?)null : decimal.Parse(element.Value);
        }

        static Project getProject(XElement element)
        {
            var id = getInt(element.Element("ixProject"));
            if (id == 0) return null;
            return new Project
                {
                    Id = id,
                    Name = getString(element.Element("sProject"))
                };
        }

        static bool getBool(XElement element)
        {
            return element != null && bool.Parse(element.Value);
        }

        static string[] getStrings(IEnumerable<XElement> elements)
        {
            return elements == null ? new string[0] : elements.Select(e => e.Value).ToArray();
        }

        static CaseId[] getCaseIds(XElement element)
        {
            return getString(element)
                .Split(',')
                .Select(s => (CaseId)int.Parse(s)).ToArray();
        }

        static string getString(XElement element)
        {
            return element == null ? string.Empty : element.Value;
        }

        static int getInt(XElement element)
        {
            return element == null ? 0 : int.Parse(element.Value);
        }

        public Case[] Cases { get; private set; }
    }
}
