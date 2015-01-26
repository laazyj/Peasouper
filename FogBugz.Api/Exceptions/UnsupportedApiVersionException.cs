using System;

namespace FogBugz.Api.Exceptions
{
    public class UnsupportedApiVersionException : FogBugzException
    {
        const string MessageFormat =
            "The FogBugz Api found at '{0}' is version {1} which requires minimum client support for version {2}. This library only supports version {3}.";

        public UnsupportedApiVersionException(string fogbugzUri, int apiVersion, int minVersion, int supportedApiVersion)
            : base(string.Format(MessageFormat, fogbugzUri, apiVersion, minVersion, supportedApiVersion))
        {            
        }
    }
}