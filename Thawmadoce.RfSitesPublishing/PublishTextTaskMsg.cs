using System;

namespace Thawmadoce.RfSitesPublishing
{
    public class PublishTextTaskMsg
    {
        public string Title { get; set; }

        public DateTime PublishDate { get; set; }

        public string Server { get; set; }

        public string Token { get; set; }
    }
}