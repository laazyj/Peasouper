using System;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using NUnit.Framework;
using Peasouper.Domain;
using Peasouper.Parsers;

namespace Peasouper.Tests.Parsers
{
    [TestFixture]
    public class CaseParserTest
    {
        [Test]
        public void ParseSampleCase()
        {
            var subject = new CaseParser();
            subject.Parse(XElement.Parse(@"
<response>
    <cases count=""1"">
      <case ixBug=""123"" operations=""edit,assign,resolve,reactivate,close,reopen,reply,forward,email,move,spam,remind"">
        <ixBug>123</ixBug> 
        <ixBugParent>234</ixBugParent>
        <ixBugChildren>456,876</ixBugChildren>
        <tags>
          <tag><![CDATA[first]]></tag>
          <tag><![CDATA[second]]></tag>
          <tag><![CDATA[third]]></tag>
        </tags>
        <fOpen>true</fOpen> 
        <sTitle>Duck, Duck... but No Goose!</sTitle> 
        <sOriginalTitle>Problem finding the goose...</sOriginalTitle> 
        <sLatestTextSummary>I searched the docs, but no goose!</sLatestTextSummary> 
        <ixBugEventLatestText>1151</ixBugEventLatestText> 
        <ixProject>22</ixProject> 
        <sProject>The Farm</sProject> 
        <ixArea>35</ixArea> 
        <sArea>Pond</sArea> 
        <ixGroup>6</ixGroup>
        <ixPersonAssignedTo>1</ixPersonAssignedTo> 
        <sPersonAssignedTo>Old MacDonald</sPersonAssignedTo> 
        <sEmailAssignedTo>grandpa@oldmacdonald.com</sEmailAssignedTo> 
        <ixPersonOpenedBy>2</ixPersonOpenedBy> 
        <ixPersonResolvedBy>2</ixPersonResolvedBy> 
        <ixPersonClosedBy></ixPersonClosedBy> 
        <ixPersonLastEditedBy>0</ixPersonLastEditedBy> 
        <ixStatus>2</ixStatus> 
        <ixBugDuplicates>321</ixBugDuplicates> 
        <ixBugOriginal>654</ixBugOriginal> 
        <sStatus>Geschlossen (Fixed)</sStatus> 
        <ixPriority>3</ixPriority> 
        <sPriority>Must Fix</sPriority> 
        <ixFixFor>3</ixFixFor> 
        <sFixFor>Test</sFixFor> 
        <dtFixFor>2007-05-06T22:47:59Z</dtFixFor> 
        <sVersion></sVersion> 
        <sComputer></sComputer> 
        <hrsOrigEst>0</hrsOrigEst> 
        <hrsCurrEst>0</hrsCurrEst> 
        <hrsElapsed>0</hrsElapsed> 
        <c>0</c> 
        <sCustomerEmail></sCustomerEmail> 
        <ixMailbox>0</ixMailbox> 
        <ixCategory>1</ixCategory> 
        <sCategory>Feature</sCategory> 
        <dtOpened>2007-05-06T22:47:59Z</dtOpened> 
        <dtResolved>2007-05-06T22:47:59Z</dtResolved> 
        <dtClosed>2007-05-06T22:47:59Z</dtClosed> 
        <ixBugEventLatest>1151</ixBugEventLatest> 
        <dtLastUpdated>2007-05-06T22:47:59Z</dtLastUpdated> 
        <fReplied>false</fReplied> 
        <fForwarded>false</fForwarded> 
        <sTicket></sTicket> 
        <ixDiscussTopic>0</ixDiscussTopic> 
        <dtDue></dtDue> 
        <sReleaseNotes></sReleaseNotes> 
        <ixBugEventLastView>1151</ixBugEventLastView> 
        <dtLastView>2007-05-06T22:47:59Z</dtLastView> 
        <ixRelatedBugs>345,267,2920</ixRelatedBugs> 
        <sScoutDescription>Main.cpp:165</sScoutDescription> 
        <sScoutMessage>Please contact us or visit our knowledge base to resolve.</sScoutMessage> 
        <fScoutStopReporting>false</fScoutStopReporting> 
        <dtLastOccurrence>2007-05-06T22:47:59Z</dtLastOccurrence> 
        <fSubscribed>true</fSubscribed> 
      </case>
    </cases>
</response>"));

            Assert.IsNotNull(subject.Cases);
            Assert.AreEqual(1, subject.Cases.Count());

            var result = subject.Cases.First();
            Assert.AreEqual((CaseId)123, result.Id);
            Assert.AreEqual((CaseId)234, result.Parent);
            Assert.AreEqual(2, result.Children.Count());
            Assert.AreEqual((CaseId)456, result.Children.First());
            Assert.AreEqual((CaseId)876, result.Children.Last());
            result.Duplicates.ShouldBeEquivalentTo(new[] { (CaseId)321 });
            result.Related.ShouldBeEquivalentTo(new[] { (CaseId)345, (CaseId)247, (CaseId)2920 });
            Assert.AreEqual((CaseId)654, result.Original);

            result.Tags.ShouldBeEquivalentTo(new[] { "first", "second", "third" });
            Assert.IsTrue(result.IsOpen);
            Assert.AreEqual("Duck, Duck... but No Goose!", result.Title);
            Assert.AreEqual("Problem finding the goose...", result.OriginalTitle);
            Assert.AreEqual("I searched the docs, but no goose!", result.LatestSummary);
            Assert.AreEqual((EventId)1151, result.LatestTextEvent);
            Assert.AreEqual((EventId)1151, result.LatestEvent);
            Assert.AreEqual((EventId)1151, result.LastViewedEvent);

            Assert.IsNotNull(result.Project);
            var project = result.Project;
            Assert.AreEqual(22, project.Id);
            Assert.AreEqual("The Farm", project.Name);

            Assert.IsNotNull(result.Area);
            var area = result.Area;
            Assert.AreEqual(35, area.Id);
            Assert.AreEqual("Pond", area.Name);

            Assert.IsNotNull(result.AssignedTo);
            var person = result.AssignedTo;
            Assert.AreEqual((PersonId)1, person.Id);
            Assert.AreEqual("Old MacDonald", person.FullName);
            Assert.AreEqual("grandpa@oldmacdonald.com", person.Email);

            Assert.AreEqual((PersonId)2, result.OpenedBy);
            Assert.AreEqual((PersonId)2, result.ResolvedBy);
            Assert.IsNull(result.ClosedBy);
            Assert.IsNull(result.LastEditedBy);

            Assert.IsNotNull(result.Status);
            Assert.AreEqual((StatusId)2, result.Status.Id);
            Assert.AreEqual("Geschlossen (Fixed)", result.Status.Name);

            Assert.IsNotNull(result.Priority);
            Assert.AreEqual(3, result.Priority.Id);
            Assert.AreEqual("Must Fix", result.Priority.Name);

            Assert.IsNotNull(result.FixFor);
            Assert.AreEqual(3, result.FixFor.Id);
            Assert.AreEqual("Test", result.FixFor.Name);
            Assert.IsNotNull(result.FixFor.Date);
// ReSharper disable once PossibleInvalidOperationException
            Assert.AreEqual(new DateTime(2007, 05, 06, 22, 47, 59, DateTimeKind.Utc), result.FixFor.Date.Value);

            Assert.IsEmpty(result.Version);
            Assert.IsEmpty(result.Computer);
            Assert.AreEqual(0, result.EstimateHoursOriginal);
            Assert.AreEqual(0, result.EstimateHoursCurrent);
            Assert.AreEqual(0, result.ElapsedHours);
            Assert.IsEmpty(result.CustomerEmail);
            Assert.IsNull(result.Mailbox);

            Assert.IsNotNull(result.Category);
            Assert.AreEqual(1, result.Category.Id);
            Assert.AreEqual("Feature", result.Category.Name);

            Assert.AreEqual(new DateTime(2007, 05, 06, 22, 47, 59, DateTimeKind.Utc), result.OpenedDate);
            Assert.AreEqual(new DateTime(2007, 05, 06, 22, 47, 59, DateTimeKind.Utc), result.ResolvedDate);
            Assert.AreEqual(new DateTime(2007, 05, 06, 22, 47, 59, DateTimeKind.Utc), result.ClosedDate);
            Assert.AreEqual(new DateTime(2007, 05, 06, 22, 47, 59, DateTimeKind.Utc), result.LastUpdatedDate);
            Assert.AreEqual(new DateTime(2007, 05, 06, 22, 47, 59, DateTimeKind.Utc), result.LastViewedDate);

            Assert.IsFalse(result.Replied);
            Assert.IsFalse(result.Forwarded);
            Assert.IsEmpty(result.Ticket);
            Assert.IsNull(result.DiscussionTopic);
            Assert.IsNull(result.DueDate);
            Assert.IsEmpty(result.ReleaseNotes);
            Assert.IsTrue(result.Subscribed);

            Assert.AreEqual("Main.cpp:165", result.ScoutDescription);
            Assert.AreEqual("Please contact us or visit our knowledge base to resolve.", result.ScoutMessage);
            Assert.IsFalse(result.ScoutStopReporting);
            Assert.AreEqual(new DateTime(2007, 05, 06, 22, 47, 59, DateTimeKind.Utc), result.ScoutLastOccurrence);
        }
    }
}
