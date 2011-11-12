using System;
using System.Diagnostics;
using System.Dynamic;
using System.Net;
using EasyHttp.Http;
using Thawmadoce.Editor;
using Thawmadoce.Extensibility;

namespace Thawmadoce.RfSitesPublishing
{
    public class PublishingSaga : ISaga
    {
        private string _lastCapturedMarkdown;

        public PublishingSaga()
        {
            
        }

        public void Handle(NewMarkdownTaskMsg msg)
        {
            _lastCapturedMarkdown = msg.MarkdownText;
        }

        public void Handle(PublishTextTaskMsg info)
        {
            Debug.WriteLine("Shacka");
            var pub = GetPublishObject(info);

            try
            {
                var http = new HttpClient();
                http.Request.AddExtraHeader("X-RfSite-AdminToken", info.Token);
                var response = http.Post(info.Server + "/admin/post", pub, HttpContentTypes.ApplicationJson);
                if (response.StatusCode == HttpStatusCode.Created)
                {

                    //Cool
                }
            }
            catch(Exception x)
            {
                Debug.WriteLine("Issue");
            }
        }

        private object GetPublishObject(PublishTextTaskMsg info)
        {
            return new
                       {
                           title = info.Title,
                           publishdate = info.PublishDate,
                           isMarkdown = true,
                           body = _lastCapturedMarkdown
                       };
        }
    }
}