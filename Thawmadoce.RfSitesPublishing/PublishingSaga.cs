using System;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using Thawmadoce.Editor;
using Thawmadoce.Extensibility;
using Thawmadoce.MainApp;

namespace Thawmadoce.RfSitesPublishing
{
    public class PublishingSaga : ISaga
    {
        private readonly IMessagePublisher _publisher;
        private string _lastCapturedMarkdown;

        public PublishingSaga(IMessagePublisher publisher)
        {
            _publisher = publisher;
        }

        public void Handle(NewMarkdownTaskMsg msg)
        {
            _lastCapturedMarkdown = msg.MarkdownText;
        }

        public void Handle(PublishTextTaskMsg info)
        {
            try
            {
                var postData = GetPublishData(info);
                var r = BuildPostRequest(info);
                r.ContentLength = postData.Length;
                var s = r.GetRequestStream();
                s.Write(postData, 0, postData.Length);
                s.Close();
                var response = (HttpWebResponse)r.GetResponse();
                HandleResponse(response);
            }
            catch(Exception x)
            {
                _publisher.Publish(new ExceptionMsg(x));
            }
        }

        private void HandleResponse(HttpWebResponse response)
        {
            if (response.StatusCode == HttpStatusCode.Created)
            {
                var location = response.Headers["Location"];
                _publisher.Publish(new AlertMsg(AlertType.Information, "Publish successful", location));
            }
        }

        private static HttpWebRequest BuildPostRequest(PublishTextTaskMsg info)
        {
            var r = (HttpWebRequest)HttpWebRequest.Create(info.Server + "/admin/post");
            r.Headers.Add("X-RfSite-AdminToken", info.Token);
            r.ContentType = "application/json";
            r.Method = "POST";
            return r;
        }

        private byte[] GetPublishData(PublishTextTaskMsg info)
        {
            var obj = new
                       {
                           title = info.Title,
                           publishdate = info.PublishDate,
                           isMarkdown = true,
                           body = _lastCapturedMarkdown
                       };
            var ser = new JavaScriptSerializer();
            var json = ser.Serialize(obj);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}