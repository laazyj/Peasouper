using System;
using System.Xml.Linq;

namespace Peasouper.Exceptions
{
    public class FogBugzException : Exception
    {
        public FogBugzException() { }

        public FogBugzException(string message) : base(message) { }

        public FogBugzException(string message, int errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public FogBugzException(string message, int errorCode, XElement response) : base(message)
        {
            ErrorCode = errorCode;
            Response = response;
        }

        public int ErrorCode { get; private set; }
        public XElement Response { get; set; }
    }
}
