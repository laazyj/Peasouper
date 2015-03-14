using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Peasouper.Domain;

namespace Peasouper.Parsers
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
            // ReSharper disable once UseObjectOrCollectionInitializer
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
                Name = getString(element.Element(CaseColumns.Area))
            };
            result.AssignedTo = new Person
            {
                Id = (PersonId)(getInt(element.Element(CaseColumns.AssignedToPersonId))),
                FullName = getString(element.Element(CaseColumns.AssignedTo)),
                Email = getString(element.Element(CaseColumns.AssignedToEmail))
            };
            result.OpenedBy = (PersonId)getInt(element.Element(CaseColumns.OpenedByPersonId));
            result.ResolvedBy = PersonId.FromInt(getInt(element.Element(CaseColumns.ResolvedByPersonId)));
            result.ClosedBy = PersonId.FromInt(getInt(element.Element(CaseColumns.ClosedByPersonId)));
            result.LastEditedBy = PersonId.FromInt(getInt(element.Element(CaseColumns.LastEditedByPersonId)));
            result.Status = new Status
            {
                Id = (StatusId)getInt(element.Element(CaseColumns.StatusId)),
                Name = getString(element.Element(CaseColumns.Status))
            };
            result.Priority = new Priority
            {
                Id = getInt(element.Element(CaseColumns.PriorityId)),
                Name = getString(element.Element(CaseColumns.Priority))
            };
            result.FixFor = new Milestone
            {
                Id = (MilestoneId)getInt(element.Element(CaseColumns.FixForMilestoneId)),
                Name = getString(element.Element(CaseColumns.FixFor)),
                Date = getDate(element.Element(CaseColumns.FixForDate))
            };
            result.Version = getString(element.Element(CaseColumns.Version));
            result.Computer = getString(element.Element(CaseColumns.Computer));
            result.EstimateHoursOriginal = getDecimal(element.Element(CaseColumns.EstimateHoursOriginal));
            result.EstimateHoursCurrent = getDecimal(element.Element(CaseColumns.EstimateHoursCurrent));
            result.ElapsedHours = getDecimal(element.Element(CaseColumns.ElapsedHours));
            result.Occurrences = getInt(element.Element(CaseColumns.Occurrences)) + 1;
            result.CustomerEmail = getString(element.Element(CaseColumns.CustomerEmail));
            result.Mailbox = MailboxId.FromInt(getInt(element.Element(CaseColumns.MailboxId)));
            result.Category = new Category
            {
                Id = getInt(element.Element(CaseColumns.CategoryId)),
                Name = getString(element.Element(CaseColumns.Category))
            };
            result.OpenedDate = getDate(element.Element(CaseColumns.OpenedDate));
            result.ResolvedDate = getDate(element.Element(CaseColumns.ResolvedDate));
            result.ClosedDate = getDate(element.Element(CaseColumns.ClosedDate));
            result.LatestEvent = EventId.FromInt(getInt(element.Element(CaseColumns.LatestEventId)));
            result.LastUpdatedDate = getDate(element.Element(CaseColumns.LastUpdatedDate));
            result.Replied = getBool(element.Element(CaseColumns.Replied));
            result.Forwarded = getBool(element.Element(CaseColumns.Forwarded));
            result.Ticket = getString(element.Element(CaseColumns.Ticket));
            result.DiscussionTopic = DiscussionId.FromInt(getInt(element.Element(CaseColumns.DiscussionTopicId)));
            result.DueDate = getDate(element.Element(CaseColumns.DueDate));
            result.ReleaseNotes = getString(element.Element(CaseColumns.ReleaseNotes));
            result.LastViewedEvent = EventId.FromInt(getInt(element.Element(CaseColumns.LastViewedEventId)));
            result.LastViewedDate = getDate(element.Element(CaseColumns.LastViewedDate));
            result.ScoutDescription = getString(element.Element(CaseColumns.ScoutDescription));
            result.ScoutMessage = getString(element.Element(CaseColumns.ScoutMessage));
            result.ScoutStopReporting = getBool(element.Element(CaseColumns.ScoutStopReporting));
            result.ScoutLastOccurrence = getDate(element.Element(CaseColumns.ScoutLastOccurrence));
            result.Subscribed = getBool(element.Element(CaseColumns.Subscribed));

            //var operations = element.Attribute("operations");
            return result;
        }

        private static string[] getTags(XElement caseElement)
        {
            var tagsElement = caseElement.Element(CaseColumns.Tags);
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
            var id = getInt(element.Element(CaseColumns.ProjectId));
            if (id == 0) return null;
            return new Project
                {
                    Id = id,
                    Name = getString(element.Element(CaseColumns.Project))
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
