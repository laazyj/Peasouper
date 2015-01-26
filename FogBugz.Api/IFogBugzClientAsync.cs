using System.Threading.Tasks;

namespace FogBugz.Api
{
    interface IFogBugzClientAsync
    {
        Task Login(string login, string password);
        Task Logout();
    }
}
