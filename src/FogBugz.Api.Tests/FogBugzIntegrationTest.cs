using System;
using System.Configuration;
using System.Diagnostics;
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
	            Assert.GreaterOrEqual(filters.Count(), 2);
	        }
	        catch
	        {
	            subject.Logout();
	            throw;
	        }
	    }

        /// <summary>
        /// NOTE: There are some limitations in the listFilters API related to returning the current filter:
        ///   1. Two filters that are identical but with different names cannot be distinguished. It's possible none of them will come back flagged as "current".
        ///   2. There are some undocumented restrictions around filter names. "integration-test-1" and "integration-test-2" would not work.
        /// Fog Creek have case FC2855671 for this.
        /// </summary>
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

        [Test]
        public void CaseDetailsTest()
        {
            var subject = getIntegrationTestClient();
            loginAsIntegrationUser(subject);
            try
            {
                var c = subject.GetCase((CaseId)1);
                // Case #1 is always a welcome to FB case...
                Assert.IsNotNull(c, "No case returned.");
                Assert.AreEqual(1, c.Id);
                Assert.AreEqual("\"Welcome to FogBugz\" Sample Case", c.Title);
                Assert.AreEqual("Inbox", c.Project.Name);
                Assert.AreEqual("Not Spam", c.Area.Name);
                Assert.AreEqual("Undecided", c.FixFor.Name);
                Assert.AreEqual("Bug", c.Category.Name);
                Assert.AreEqual("Active", c.Status.Name);
                Assert.AreEqual("Must Fix", c.Priority.Name);
                Assert.IsNotNull(c.AssignedTo);
                Assert.IsNotNull(c.AssignedTo.Id);
                Assert.IsNotNull(c.AssignedTo.FullName);
                Debug.WriteLine("Case assigned to: " + c.AssignedTo.FullName);
            }
            finally
            {
                subject.Logout();
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

