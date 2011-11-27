using System.IO;
using System.Net;
using System.Web.Script.Serialization;

namespace Thawmadoce.RfSitesPublishing
{
    public static class WebrequestExtensions
    {
        public static string GetResponse(this WebResponse response)
        {
            using (var sr = new StreamReader(response.GetResponseStream()))
                return sr.ReadToEnd();
        }
    }
}