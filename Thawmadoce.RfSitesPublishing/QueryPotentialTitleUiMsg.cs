using System;

namespace Thawmadoce.RfSitesPublishing
{
    public class QueryPotentialTitleUiMsg
    {
        private readonly Action<string> _continuation;

        public QueryPotentialTitleUiMsg(Action<string> continuation)
        {
            _continuation = continuation;
        }

        public void SetTitle(string potentialTitle)
        {
            _continuation(potentialTitle);
        }
    }
}