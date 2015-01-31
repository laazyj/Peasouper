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
                    OpenedBy = element.Element("ixPersonOpenedBy"),
                    ResolvedBy = element.Element("ixPersonResolvedBy"),
                    ClosedBy = element.Element("ixPersonClosedBy"),
                    LastEditedBy = element.Element("ixPersonLastEditedBy"),
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
                    Version = element.Element("sVersion"),
                    Computer = element.Element("sComputer"),
                    EstimateHoursOriginal = element.Element("hrsOrigEst"),
                    EstimateHoursCurrent = element.Element("hrsCurrEst"),
                    ElapsedHours = element.Element("hrsElapsed"),
                    Occurrences = element.Element("c") + 1,
                    CustomerEmail = element.Element("sCustomerEmail"),
                    Mailbox = element.Element("ixMailbox"),
                    Category = new Category
                        {
                            Id = element.Element("ixCategory"),
                            Name = element.Element("sCategory")
                        },
                    OpenedDate = element.Element("dtOpened"),
                    ResolvedDate = element.Element("dtResolved"),
                    ClosedDate = element.Element("dtClosed"),
                    LatestEvent = element.Element("ixBugEventLatest"),
                    LastUpdatedDate = element.Element("dtLastUpdated"),
                    Replied = element.Element("fReplied"),
                    Forwarded = element.Element("fForwarded"),
                    Ticket = element.Element("sTicket"),
                    DiscussionTopic = element.Element("ixDiscussTopic"),
                    DueDate = element.Element("dtDue"),
                    ReleaseNotes = element.Element("sReleaseNotes"),
                    LastViewedEvent = element.Element("ixBugEventLastView"),
                    LastViewedDate = element.Element("dtLastView"),
                    ScoutDescription = element.Element("sScoutDescription"),
                    ScoutMessage = element.Element("sScountMessage"),
                    ScoutStopReporting = element.Element("fScoutStopReporting"),
                    ScoutLastOccurrence = element.Element("dtLastOccurrence"),
                    Subscribed = element.Element("fSubscribed")
                };
            var operations = element.Attribute("operations");
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
