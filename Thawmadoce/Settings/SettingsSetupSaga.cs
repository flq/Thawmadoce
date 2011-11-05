using System;
using System.Configuration;
using MemBus;
using MemBus.Subscribing;
using Thawmadoce.Extensibility;
using Thawmadoce.Frame.Messaging;
using Thawmadoce.MainApp;

namespace Thawmadoce.Settings
{
    public class SettingsSetupSaga : ISaga, IAcceptDisposeToken
    {
        private readonly IPublisher _publisher;
        private IDisposable _disposeToken;

        public SettingsSetupSaga(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public void Handle(UiSystemReadyUiMsg msg)
        {
            var cfg = GetCfg();
            var x = cfg.AppSettings.Settings["config_dir"];
            if (x == null)
                _publisher.Publish(new ActivateAppDialog(typeof(SettingsViewModel)));
        }

        public void Handle(SettingsStorageLocationSetTaskMsg msg)
        {
            var cfg = GetCfg();
            cfg.AppSettings.Settings.Add("config_dir", msg.SettingsDirectory);
            cfg.Save(ConfigurationSaveMode.Modified);
            _publisher.Publish(new RefocusEditorUiMsg());
            _disposeToken.Dispose();
        }

        public void Accept(IDisposable disposeToken)
        {
            _disposeToken = disposeToken;
        }

        private static Configuration GetCfg()
        {
            return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }
    }
}