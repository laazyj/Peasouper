using System.Collections.Generic;
using Peasouper.Domain;

namespace Peasouper
{
    public interface IFogBugzClient
    {
        void Login(string login, string password);
        void Logout();

        IEnumerable<Filter> GetFilters();
        void SetFilter(Filter filter);
        void SetFilter(FilterId id);

        IEnumerable<Case> GetCases(string query, string[] columns, int? maxRecords);
        Case GetCase(CaseId id);
        Case GetCase(CaseId id, string[] columns);
    }
}
