using System;
using System.Collections.ObjectModel;
using Thawmadoce.Settings;
using Thawmadoce.Frame.Extensions;
using System.Linq;

namespace Thawmadoce.RfSitesPublishing
{
    public class PublishingViewModel
    {
        private const string StoreKey = "PublishingViewModel.Servers";
        
        private readonly ISettings _settings;
        private readonly ObservableCollection<ServerModel> _servers = new ObservableCollection<ServerModel>();

        public PublishingViewModel(ISettings settings)
        {
            _settings = settings;
            PublishDate = DateTime.Today;
            Time = DateTime.UtcNow.ToShortTimeString();
            LoadStoredServers();
        }

        public string Title { get; set; }
        public string Time { get; set; }
        public DateTime PublishDate { get; set; }

        public ObservableCollection<ServerModel> Servers
        {
            get { return _servers; }
        }

        public ServerModel CurrentServer { get; set; }

        public void AddServer()
        {
            var serverViewModel = new ServerModel { CanEdit = true};
            serverViewModel.Saved += HandleSaved;
            Servers.Add(serverViewModel);
        }

        private void LoadStoredServers()
        {
            var knownServers = _settings.Get<dynamic[]>(StoreKey);
            if (knownServers != null)
            {
                _servers.AddRange(
                    knownServers
                        .Select(o => new ServerModel(o))
                        .Pipeline(sm => sm.Saved += HandleSaved));
            }
        }

        private void HandleSaved(object sender, EventArgs e)
        {
            _settings.Post(StoreKey, Servers.Select(s => s.Compress()).ToArray());
        }
    }
}