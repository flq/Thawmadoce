﻿using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Thawmadoce.Extensibility;
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
        private readonly ObservableCollection<ServerModel> _servers = new ObservableCollection<ServerModel>();

        public PublishingViewModel(ISettings settings, IMessagePublisher publisher)
        {
            _settings = settings;
            _publisher = publisher;
            PublishDate = DateTime.Today;
            Time = DateTime.UtcNow.ToShortTimeString();
            LoadStoredServers();
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