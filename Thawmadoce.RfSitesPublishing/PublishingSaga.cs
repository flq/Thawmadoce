using System;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using Thawmadoce.Editor;
using Thawmadoce.Extensibility;
using Thawmadoce.MainApp;
using System.Linq;

namespace Thawmadoce.RfSitesPublishing
{
    public class PublishingSaga : ISaga
    {
        private readonly IMessagePublisher _publisher;
        private string _lastCapturedMarkdown;
        private string _potentialTitle;

        public PublishingSaga(IMessagePublisher publisher)
        {
            _publisher = publisher;
        }

        public void Handle(NewMarkdownTaskMsg msg)
        {
            _lastCapturedMarkdown = msg.MarkdownText;
        }

        public void Handle(NewDisplayNameUiMsg msg)
        {
            if (!msg.IsTitleReset && msg.NewTitle.EndsWith(".md"))
                _potentialTitle = msg.NewTitle;
        }

        public void Handle(QueryPotentialTitleUiMsg msg)
        {
            if (_potentialTitle != null)
                msg.SetTitle(_potentialTitle.Substring(0, _potentialTitle.Length -3));
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
                           tags = GetTags(info.Tags),
                           publishdate = info.PublishDate,
                           isMarkdown = true,
                           body = _lastCapturedMarkdown
                       };
            var ser = new JavaScriptSerializer();
            var json = ser.Serialize(obj);
            return Encoding.UTF8.GetBytes(json);
        }

        private static string[] GetTags(string tags)
        {
            return tags.Split(',').Select(s => s.Trim()).ToArray();
        }
    }
}