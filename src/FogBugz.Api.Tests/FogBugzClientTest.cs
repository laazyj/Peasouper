using FakeItEasy;
using NUnit.Framework;

namespace FogBugz.Api.Tests
{
    [TestFixture]
    public class FogBugzClientTest
    {
        public void SetCurrentFilterTests()
        {
            var httpClient = A.Fake<IFogBugzHttpClient>();
            var subject = new FogBugzClient("url") {HttpClient = httpClient};
        }
    }
}
