using System.Linq;
using System.Xml.Linq;

namespace Peasouper.Exceptions
{
    internal class AmbiguousLogonException : FogBugzException
    {
        public string[] People { get; private set; }

        public AmbiguousLogonException(string message, XElement response)
            : base(ErrorMessage, 2, response)
        {
            var xpeople = response.Element("people");
            if (xpeople == null) return;
            var xpersons = xpeople.Elements("person");
            if (xpersons == null) return;
            People = xpersons.Select(x => x.Value).ToArray();
        }

        const string ErrorMessage =
            "Ambiguous Logon Error. Multiple accounts were found with the email address provided. Please specify the full name your account.";
    }
}