namespace Thawmadoce.RfSitesPublishing
{
    public class LoadContentTaskMsg
    {
        private readonly EnterIdArgs _args;

        public LoadContentTaskMsg(EnterIdArgs args)
        {
            _args = args;
        }

        public string Id { get { return _args.Id; } }
        public string Server { get { return _args.Server; } }
        public string Token { get { return _args.Token; } }
    }
}