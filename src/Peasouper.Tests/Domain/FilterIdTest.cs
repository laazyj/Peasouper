using NUnit.Framework;
using Peasouper.Domain;

namespace Peasouper.Tests.Domain
{
    [TestFixture]
    public class FilterIdTest
    {
        [Test]
        public void EqualityTest()
        {
            var filter1 = (FilterId)"f1";
            var filter2 = (FilterId)"f2";
            var filter3 = (FilterId)"f1";

            Assert.AreEqual(filter1, filter3);
            Assert.AreEqual(filter1, "f1");
            Assert.IsTrue("f1" == filter3);
            Assert.AreEqual(filter1.GetHashCode(), filter3.GetHashCode());
            Assert.AreNotEqual(filter1, filter2);
            Assert.AreNotEqual(filter1.GetHashCode(), filter2.GetHashCode());
            Assert.AreNotEqual(filter1, "something");
        }
    }
}
