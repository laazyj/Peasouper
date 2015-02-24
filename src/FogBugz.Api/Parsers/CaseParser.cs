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
                Id = (CaseId)int.Parse(element.Attribute("ixBug").Value)
            };
            result.Parent = CaseId.FromInt(getInt(element.Element(CaseColumns.Parent)));
            result.Children = getCaseIds(element.Element(CaseColumns.Children));
            result.Duplicates = getCaseIds(element.Element(CaseColumns.Duplicates));
            result.Original = CaseId.FromInt(getInt(element.Element(CaseColumns.Original)));
            result.Related = getCaseIds(element.Element(CaseColumns.Related));
            result.Tags = getTags(element); // TODO: Write an integration test for tags.
            result.IsOpen = getBool(element.Element(CaseColumns.IsOpen));
            result.Title = getString(element.Element(CaseColumns.Title));
            result.OriginalTitle = getString(element.Element(CaseColumns.OriginalTitle));
            result.LatestSummary = getString(element.Element(CaseColumns.LatestSummary));
            result.LatestTextEvent = EventId.FromInt(getInt(element.Element(CaseColumns.LatestTextEvent)));
            result.Project = getProject(element);
            result.Area = new Area
            {
                Id = getInt(element.Element(CaseColumns.AreaId)),
                Name = getString(element.Element("sArea"))
            };
            result.AssignedTo = new Person
            {
                Id = (PersonId)(getInt(element.Element("ixPersonAssignedTo"))),
                FullName = getString(element.Element("sPersonAssignedTo")),
                Email = getString(element.Element("sEmailAssignedTo"))
            };
            result.OpenedBy = (PersonId)getInt(element.Element("ixPersonOpenedBy"));
            result.ResolvedBy = PersonId.FromInt(getInt(element.Element("ixPersonResolvedBy")));
            result.ClosedBy = PersonId.FromInt(getInt(element.Element("ixPersonClosedBy")));
            result.LastEditedBy = PersonId.FromInt(getInt(element.Element("ixPersonLastEditedBy")));
            result.Status = new Status
            {
                Id = (StatusId)getInt(element.Element("ixStatus")),
                Name = getString(element.Element("sStatus"))
            };
            result.Priority = new Priority
            {
                Id = getInt(element.Element("ixPriority")),
                Name = getString(element.Element("sPriority"))
            };
            result.FixFor = new Milestone
            {
                Id = (MilestoneId)getInt(element.Element("ixFixFor")),
                Name = getString(element.Element("sFixFor")),
                Date = getDate(element.Element("dtFixFor"))
            };
            result.Version = getString(element.Element("sVersion"));
            result.Computer = getString(element.Element("sComputer"));
            result.EstimateHoursOriginal = getDecimal(element.Element("hrsOrigEst"));
            result.EstimateHoursCurrent = getDecimal(element.Element("hrsCurrEst"));
            result.ElapsedHours = getDecimal(element.Element("hrsElapsed"));
            result.Occurrences = getInt(element.Element("c")) + 1;
            result.CustomerEmail = getString(element.Element("sCustomerEmail"));
            result.Mailbox = MailboxId.FromInt(getInt(element.Element("ixMailbox")));
            result.Category = new Category
            {
                Id = getInt(element.Element("ixCategory")),
                Name = getString(element.Element("sCategory"))
            };
            result.OpenedDate = getDate(element.Element("dtOpened"));
            result.ResolvedDate = getDate(element.Element("dtResolved"));
            result.ClosedDate = getDate(element.Element("dtClosed"));
            result.LatestEvent = EventId.FromInt(getInt(element.Element("ixBugEventLatest")));
            result.LastUpdatedDate = getDate(element.Element("dtLastUpdated"));
            result.Replied = getBool(element.Element("fReplied"));
            result.Forwarded = getBool(element.Element("fForwarded"));
            result.Ticket = getString(element.Element("sTicket"));
            result.DiscussionTopic = DiscussionId.FromInt(getInt(element.Element("ixDiscussTopic")));
            result.DueDate = getDate(element.Element("dtDue"));
            result.ReleaseNotes = getString(element.Element("sReleaseNotes"));
            result.LastViewedEvent = EventId.FromInt(getInt(element.Element("ixBugEventLastView")));
            result.LastViewedDate = getDate(element.Element("dtLastView"));
            result.ScoutDescription = getString(element.Element("sScoutDescription"));
            result.ScoutMessage = getString(element.Element("sScoutMessage"));
            result.ScoutStopReporting = getBool(element.Element("fScoutStopReporting"));
            result.ScoutLastOccurrence = getDate(element.Element("dtLastOccurrence"));
            result.Subscribed = getBool(element.Element("fSubscribed"));

            var operations = element.Attribute("operations");
            return result;
        }

        private static string[] getTags(XElement caseElement)
        {
            var tagsElement = caseElement.Element("tags");
            return tagsElement == null ? new string[0] : getStrings(tagsElement.Elements("tag"));
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
