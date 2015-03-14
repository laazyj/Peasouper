using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using Peasouper.Domain;
using Peasouper.Parsers;

namespace Peasouper.Tests.Parsers
{
    [TestFixture]
    public class FiltersListParserTest
    {
        [Test]
        public void ParseSampleFiltersList()
        {
            var subject = new FiltersListParser();
            subject.Parse(XElement.Parse(@"
<response>
    <filters>
        <filter type=""builtin"" sFilter=""ez349"">My Cases</filter>
        <filter type=""saved"" sFilter=""304"">Cases I should have closed months ago</filter>
        <filter type=""shared"" sFilter=""98"" status=""current"">Customer Service Top 10</filter>
    </filters>
</response>"));

            Assert.AreEqual(3, subject.Filters.Length);
            Assert.NotNull(subject.Current);
            Assert.AreEqual(subject.Filters.Last(), subject.Current);

            var filter = subject.Filters.First();
            Assert.AreEqual(FilterType.BuiltIn, filter.Type);
            Assert.AreEqual("ez349", (string)filter.Id);
            Assert.AreEqual("My Cases", filter.Name);

            filter = subject.Filters.Skip(1).First();
            Assert.AreEqual(FilterType.Saved, filter.Type);
            Assert.AreEqual("304", (string)filter.Id);
            Assert.AreEqual("Cases I should have closed months ago", filter.Name);

            filter = subject.Filters.Last();
            Assert.AreEqual(FilterType.Shared, filter.Type);
            Assert.AreEqual("98", (string)filter.Id);
            Assert.AreEqual("Customer Service Top 10", filter.Name);
        }

        [Test]
        public void OverwriteResultsOnSubsequentParse()
        {
            var subject = new FiltersListParser();
            subject.Parse(XElement.Parse(@"
<response>
    <filters>
        <filter type=""saved"" sFilter=""1"" status=""current"">Test One</filter><filter type=""builtin"" sFilter=""2"">Test Two</filter>
    </filters>
</response>"));
            Assert.AreEqual(2, subject.Filters.Length);
            Assert.AreEqual("1", (string)subject.Filters.First().Id);
            Assert.NotNull(subject.Current);

            // Ensure Current is reset if not provided.
            subject.Parse(XElement.Parse(@"<response><filters><filter type=""builtin"" sFilter=""something"">Test</filter></filters></response>"));
            Assert.AreEqual(1, subject.Filters.Length);
            Assert.Null(subject.Current);
        }
    }
}
