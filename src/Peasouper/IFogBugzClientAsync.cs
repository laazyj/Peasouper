using System.Threading.Tasks;

namespace Peasouper
{
    interface IFogBugzClientAsync
    {
        Task Login(string login, string password);
        Task Logout();
    }
}
