using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Thawmadoce.Extensibility;
using Thawmadoce.Frame;
using Thawmadoce.Settings;
using Thawmadoce.Frame.Extensions;
using System.Linq;

namespace Thawmadoce.RfSitesPublishing
{
    public class PublishingViewModel : IHasTitle
    {
        private const string StoreKey = "PublishingViewModel.Servers";
        
        private readonly ISettings _settings;
        private readonly IMessagePublisher _publisher;
        private readonly IUserInteraction _interaction;
        private readonly ObservableCollection<ServerModel> _servers = new ObservableCollection<ServerModel>();

        public PublishingViewModel(ISettings settings, IMessagePublisher publisher, IUserInteraction interaction)
        {
            _settings = settings;
            _publisher = publisher;
            _interaction = interaction;
            PublishDate = DateTime.Today;
            Time = DateTime.UtcNow.ToShortTimeString();
            LoadStoredServers();
            _publisher.Publish(new QueryPotentialTitleUiMsg(pt => Title = pt));
        }

        string IHasTitle.Title
        {
            get { return "RfSites Publishing"; }
        }

        public string Title { get; set; }
        public string Tags { get; set; }
        public string Time { get; set; }
        public DateTime PublishDate { get; set; }

        public ObservableCollection<ServerModel> Servers
        {
            get { return _servers; }
        }

        public ServerModel CurrentServer { get; set; }

        public DateTime PublishDateTime
        {
            get
            {
                TimeSpan ts;
                if (TimeSpan.TryParseExact(Time, "g", CultureInfo.CurrentCulture, out ts))
                    return PublishDate + ts;
                return PublishDate;
            }
        }

        public void AddServer()
        {
            var serverViewModel = new ServerModel { CanEdit = true};
            serverViewModel.Saved += HandleSaved;
            Servers.Add(serverViewModel);
        }

        public void Publish()
        {
            _publisher.Publish(new PublishTextTaskMsg
                                   {
                                       Title = Title,
                                       PublishDate = PublishDateTime, 
                                       Tags = Tags,
                                       Server = CurrentServer.Address, 
                                       Token = CurrentServer.Token
                                   });
        }

        public void IdToUpdate()
        {
            if (CurrentServer == null) return;
            var idArgs = new EnterIdArgs();
            idArgs = _interaction.Dialog<EnterIdViewModel>().Run(idArgs);
            if (idArgs.UserCancelled) 
                return;
            idArgs.Server = CurrentServer.Address;
            idArgs.Token = CurrentServer.Token;
            _publisher.Publish(new LoadContentTaskMsg(idArgs));
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