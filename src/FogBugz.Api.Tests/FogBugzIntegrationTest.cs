using System.Configuration;
using System.Linq;
using NUnit.Framework;

namespace FogBugz.Api.Tests
{
    [TestFixture]
	public class FogBugzIntegrationTest
	{
	    string _fogBugzLogin;
        readonly string _fogBugzUrl;
	    string _fogBugzPassword;
        readonly bool _integrationTestsEnabled;

        public FogBugzIntegrationTest()
	    {
	        _fogBugzUrl = ConfigurationManager.AppSettings["FogBugz.Url"];
	        _fogBugzLogin = ConfigurationManager.AppSettings["FogBugz.Login"];
	        _fogBugzPassword = ConfigurationManager.AppSettings["FogBugz.Password"];
            checkEnabled();
	    }

	    [Test]
	    public void ApiVersionCheck()
	    {
	        var subject = getIntegrationTestClient();
	        Assert.DoesNotThrow(subject.EnsureApiVersion);
	        Assert.NotNull(subject.ApiVersion);
	    }

        [Test]
		public void LoginWithValidCredentials()
	    {
	        var subject = getIntegrationTestClient();

            Assert.DoesNotThrow(() => subject.Login(_fogBugzLogin, _fogBugzPassword));
		    Assert.AreEqual(true, subject.IsLoggedIn);

		    subject.Logout();
		    Assert.AreEqual(false, subject.IsLoggedIn);
		}

	    [Test]
	    public void FiltersListTest()
	    {
	        var subject = getIntegrationTestClient();
	        loginAsIntegrationUser(subject);
	        try
	        {
	            var filters = subject.GetFilters();
                // There are always at least 2 built-in filters.
	            Assert.GreaterOrEqual(2, filters.Count());
	        }
	        catch
	        {
	            subject.Logout();
	            throw;
	        }
	    }

        FogBugzClient getIntegrationTestClient()
        {
            return new FogBugzClient(_fogBugzUrl);
        }

	    void loginAsIntegrationUser(IFogBugzClient client)
	    {
	        client.Login(_fogBugzLogin, _fogBugzPassword);
	    }

        void checkEnabled()
        {
            if (!string.IsNullOrEmpty(_fogBugzUrl)
                && !string.IsNullOrEmpty(_fogBugzLogin)
                && !string.IsNullOrEmpty(_fogBugzPassword))
                return;

            Assert.Ignore("You must provide FogBugz Url, Login & Password in AppSettings to enable integration tests.");
        }
    }
}

