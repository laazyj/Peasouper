using System.Xml.Linq;

namespace Peasouper.Exceptions
{
    internal class NotLoggedInException : FogBugzException
    {
        public NotLoggedInException(string error, XElement response)
            : base(ErrorMessage, 3, response)
        {
        }

        const string ErrorMessage =
            "FogBugz API call failed: You are not logged in. Run FogBugzClient.Login() first, or create the FogBugzClient using an existing LoginToken.";
    }
}