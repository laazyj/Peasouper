using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using Flurl;
using FogBugz.Api.Domain;
using FogBugz.Api.Exceptions;
using FogBugz.Api.Parsers;
using Serilog;

namespace FogBugz.Api
{
	public class FogBugzClient : IFogBugzClient
	{
	    public const int SupportedApiVersion = 8;

	    bool _apiVersionChecked;
	    string _fogBugzUrl;
	    Filter _currentFilter;

	    public IFogBugzHttpClient HttpClient { get; set; }
        public FiltersListParser FiltersListParser { get; set; }

	    public FogBugzClient(string fogBugzUri, string existingLoginToken = null)
	    {
	        FogBugzUri = fogBugzUri;
	        LoginToken = existingLoginToken;
	        HttpClient = new FogBugzHttpClient();
	        FiltersListParser = new FiltersListParser();
	    }

	    public string FogBugzUri
	    {
            get { return _fogBugzUrl; }
	        private set
	        {
	            _fogBugzUrl = value;
	            ApiUrl = new Url(_fogBugzUrl).AppendPathSegment("/api.asp").ToString();
	        }
	    }
	    public string LoginToken { get; private set; }
        public int? ApiVersion { get; private set; }
        protected string ApiUrl { get; private set; }

	    public bool IsLoggedIn
        {
            get
            {
                if (LoginToken == null) return false;
                EnsureApiVersion();
                return true;
            }
        }

        public void Login(string login, string password)
		{
			EnsureApiVersion();
            var uri = new Url(ApiUrl)
                .SetQueryParams(new
                    {
                        cmd = "logon",
                        email = login,
                        password
                    });
            var response = HttpClient.GetAsync(uri.ToString()).Result;
            LoginToken = response.Element("token").Value;
            Log.Information("Successfully logged in as '{0}'.", login);
        }

	    public void Logout()
	    {
	        if (!IsLoggedIn) return;
	        executeFogBugzApiCommand("logoff");
	        LoginToken = null;
            Log.Information("Successfully logged out.");
	    }

        /// <summary>
        /// Return list of available filters.
        /// </summary>
	    public IEnumerable<Filter> GetFilters()
	    {
	        var response = executeFogBugzApiCommand("listFilters");
	        FiltersListParser.Parse(response);
	        _currentFilter = FiltersListParser.Current;
	        return FiltersListParser.Filters;
	    }

	    public void SetFilter(FilterId id)
	    {
	        executeFogBugzApiCommand("setCurrentFilter", new {sFilter = id});
	        _currentFilter = null;
	    }

	    public IEnumerable<Case> GetCases(string query, string[] columns, int? maxRecords)
	    {
            // Ensure query is not parsed as a case #
	        int x;
	        if (int.TryParse(query, NumberStyles.Integer, null, out x))
	            query = string.Format("\"{0}\"", query);

	        var response = executeFogBugzApiCommand("search", new
	            {
	                q = query,
	                cols = string.Join(",", columns),
	                max = maxRecords
	            });
	        return new Case[0];
	    }

	    public Case GetCase(CaseId id)
	    {
	        var response = executeFogBugzApiCommand("search", new
	            {
	                q = id.ToString()
	            });
	        return null;
	    }

	    public void SetFilter(Filter filter)
	    {
	        SetFilter(filter.Id);
	        _currentFilter = filter;
	    }

	    /// <summary>
        /// Return the currently selected filter. This will not result in an Api call if the Filters list has already been loaded.
        /// </summary>
        /// <returns></returns>
        public Filter GetCurrentFilter()
        {
            if (_currentFilter == null)
                GetFilters();
            return _currentFilter;
        }

	    public void EnsureApiVersion()
	    {
	        if (_apiVersionChecked) return;
			if (FogBugzUri == null)
				throw new InvalidOperationException ("FogBugz URL has not been configured.");

			var uri = new Url(FogBugzUri).AppendPathSegment("/api.xml");
	        var response = HttpClient.GetAsync(uri).Result;
            
	        ApiVersion = int.Parse(response.Element("version").Value);
	        var minVersion = int.Parse(response.Element("minversion").Value);
            Log.Information("Found FogBugz Api at '{0}'. Version: {1}. MinVersion: {2}.", uri, ApiVersion, minVersion);

	        if (minVersion > SupportedApiVersion)
	            throw new UnsupportedApiVersionException(FogBugzUri, ApiVersion.Value, minVersion, SupportedApiVersion);
            _apiVersionChecked = true;
        }

	    XElement executeFogBugzApiCommand(string command, object parameters = null)
	    {
            EnsureApiVersion();
	        var uri = new Url(ApiUrl)
	            .SetQueryParams(parameters)
                .SetQueryParam("token", LoginToken)
                .SetQueryParam("cmd", command);
	        var response = HttpClient.GetAsync(uri.ToString()).Result;
	        return response;
	    }
	}
}

