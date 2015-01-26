using System.Collections.Generic;
using FogBugz.Api.Domain;

namespace FogBugz.Api
{
    public interface IFogBugzClient
    {
        void Login(string login, string password);
        void Logout();

        IEnumerable<Filter> GetFilters();
        Filter GetFilter(FilterId id);
        //void SetFilter(int filterId);

        //IEnumerable<Case> GetCases(string query, string[] columns, int maxRecords);
    }
}
