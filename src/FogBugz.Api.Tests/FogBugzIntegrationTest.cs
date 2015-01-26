using System;
using System.Configuration;
using System.Linq;
using FogBugz.Api.Domain;
using NUnit.Framework;

namespace FogBugz.Api.Tests
{
    [TestFixture]
	public class FogBugzIntegrationTest
	{
        readonly string _fogBugzLogin;
        readonly string _fogBugzUrl;
        readonly string _fogBugzPassword;

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

        [Test]
        public void SetCurrentFilterTest()
        {
            var subject = getIntegrationTestClient();
            loginAsIntegrationUser(subject);
            try
            {
                var filters = subject.GetFilters().ToArray();
                if (filters.Count(x => x.Type != FilterType.BuiltIn) < 2)
                    Assert.Ignore("Unable to run integration test, there must be at least 2 non built-in filters available.");

                var f1 = filters.First(x => x.Type != FilterType.BuiltIn);
                var f2 = filters.First(x => x != f1 && x.Type != FilterType.BuiltIn);

                subject.SetFilter(f1);
                Assert.AreEqual(f1, subject.GetCurrentFilter());
                subject.SetFilter(f2);
                Assert.AreEqual(f2, subject.GetCurrentFilter());

                // TODO: SetCurrentFilter command is not raising an error, but the filter list isn't showing the current status.
                subject.SetFilter(f1.Id);
                var current = subject.GetCurrentFilter();
                Assert.NotNull(current, "No current filter set.");
                Assert.AreEqual(f1.Id, current.Id);

                subject.SetFilter(f2.Id);
                Assert.AreEqual(f2.Id, subject.GetCurrentFilter().Id);
            }
            catch (Exception)
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

