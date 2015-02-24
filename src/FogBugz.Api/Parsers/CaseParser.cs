using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using FogBugz.Api.Domain;

namespace FogBugz.Api.Parsers
{
    public class CaseParser
    {
        public void Parse(XElement response)
        {
            var casesEle = response.Element("cases");
            if (casesEle == null)
                throw new XmlException("Expected outer element 'cases' was not found.");
            var casesArr = casesEle.Elements("case");
            Cases = casesArr.Select(parseCase).ToArray();
        }

        static Case parseCase(XElement element)
        {
            var result = new Case
                {
                    Id = (CaseId)getInt(element.Element("ixBug")),
                    Parent = CaseId.FromInt(getInt(element.Element("ixBugParent"))),
                    Children = getCaseIds(element.Element("ixBugChildren")),
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
                            Id = getInt(element.Element("ixArea")),
                            Name = getString(element.Element("sArea"))
                        },
                    AssignedTo = new Person
                        {
                            Id = (PersonId)(getInt(element.Element("ixPersonAssignedTo"))),
                            FullName = getString(element.Element("sPersonAssignedTo")),
                            Email = getString(element.Element("sEmailAssignedTo"))
                        },
                    OpenedBy = (PersonId)getInt(element.Element("ixPersonOpenedBy")),
                    ResolvedBy = PersonId.FromInt(getInt(element.Element("ixPersonResolvedBy"))),
                    ClosedBy = PersonId.FromInt(getInt(element.Element("ixPersonClosedBy"))),
                    LastEditedBy = PersonId.FromInt(getInt(element.Element("ixPersonLastEditedBy"))),
                    Status = new Status
                    {
                        Id = (StatusId)getInt(element.Element("ixStatus")),
                        Name = getString(element.Element("sStatus"))
                    },
                    Priority = new Priority
                        {
                            Id = getInt(element.Element("ixPriority")),
                            Name = getString(element.Element("sPriority"))
                        },
                    FixFor = new Milestone
                        {
                            Id = (MilestoneId)getInt(element.Element("ixFixFor")),
                            Name = getString(element.Element("sFixFor")),
                            Date = getDate(element.Element("dtFixFor"))
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
                            Id = getInt(element.Element("ixCategory")),
                            Name = getString(element.Element("sCategory"))
                        },
                    OpenedDate = getDate(element.Element("dtOpened")).Value,
                    ResolvedDate = getDate(element.Element("dtResolved")),
                    ClosedDate = getDate(element.Element("dtClosed")),
                    LatestEvent = EventId.FromInt(getInt(element.Element("ixBugEventLatest"))),
                    LastUpdatedDate = getDate(element.Element("dtLastUpdated")),
                    Replied = getBool(element.Element("fReplied")),
                    Forwarded = getBool(element.Element("fForwarded")),
                    Ticket = getString(element.Element("sTicket")),
                    DiscussionTopic = DiscussionId.FromInt(getInt(element.Element("ixDiscussTopic"))),
                    DueDate = getDate(element.Element("dtDue")),
                    ReleaseNotes = getString(element.Element("sReleaseNotes")),
                    LastViewedEvent = EventId.FromInt(getInt(element.Element("ixBugEventLastView"))),
                    LastViewedDate = getDate(element.Element("dtLastView")),
                    ScoutDescription = getString(element.Element("sScoutDescription")),
                    ScoutMessage = getString(element.Element("sScoutMessage")),
                    ScoutStopReporting = getBool(element.Element("fScoutStopReporting")),
                    ScoutLastOccurrence = getDate(element.Element("dtLastOccurrence")),
                    Subscribed = getBool(element.Element("fSubscribed"))
                };
            var operations = element.Attribute("operations");
            return result;
        }

        private static DateTime? getDate(XElement element)
        {
            if (element == null || string.IsNullOrEmpty(element.Value)) 
                return null;
            
            return DateTime.Parse(element.Value, null, DateTimeStyles.RoundtripKind);
        }

        private static decimal? getDecimal(XElement element)
        {
            return element == null || string.IsNullOrEmpty(element.Value) ? (decimal?)null : decimal.Parse(element.Value);
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
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => (CaseId)int.Parse(s)).ToArray();
        }

        static string getString(XElement element)
        {
            return element == null ? string.Empty : element.Value;
        }

        static int getInt(XElement element)
        {
            return element == null || string.IsNullOrEmpty(element.Value) ? 0 : int.Parse(element.Value);
        }

        public Case[] Cases { get; private set; }
    }
}
