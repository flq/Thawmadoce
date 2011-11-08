using System;
using System.Collections.ObjectModel;

namespace Thawmadoce.RfSitesPublishing
{
    public class PublishingViewModel
    {
        private readonly ObservableCollection<ServerViewModel> _servers = new ObservableCollection<ServerViewModel>();
        
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }

        public ObservableCollection<ServerViewModel> Servers
        {
            get { return _servers; }
        }

        public ServerViewModel CurrentServer { get; set; }

        public void AddServer()
        {
            Servers.Add(new ServerViewModel { CanEdit = true});
        }
    }
}