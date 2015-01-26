using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using FogBugz.Api.Exceptions;
using ModernHttpClient;

namespace FogBugz.Api
{
    public interface IFogBugzHttpClient
    {
        Task<XElement> GetAsync(string url);
    }

    public class FogBugzHttpClient : IFogBugzHttpClient
    {
        public async Task<XElement> GetAsync(Uri uri)
        {
            return await GetAsync(uri.AbsoluteUri);
        }

        public async Task<XElement> GetAsync(string url)
        {
            using (var httpClient = getHttpClient())
            using (var response = await httpClient.GetAsync(url).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();

                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    var xml = XElement.Load(stream);
                    checkForFogBugzError(xml);
                    return xml;
                }
            }
        }

        static HttpClient getHttpClient()
	    {
	        var client = new HttpClient(new NativeMessageHandler());
	        client.DefaultRequestHeaders.Accept.ParseAdd("text/xml");
            client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("UTF-8");
	        return client;
	    }

        void checkForFogBugzError(XElement response)
        {
            var error = response.Element("error");
            if (error == null) return;

            var errorCode = getErrorCode(error);
            switch (errorCode)
            {
                case 2:
                    throw new AmbiguousLogonException(error.Value, response);
                case 3:
                    throw new NotLoggedInException(error.Value, response);
                default:
                    throw new FogBugzException(error.Value, errorCode, response);
            }
        }

        int getErrorCode(XElement error)
        {
            var codeAttrib = error.Attribute("code");
            int codeVal;
            if (codeAttrib == null || !int.TryParse(codeAttrib.Value, out codeVal)) return 0;
            return codeVal;
        }
    }
}
