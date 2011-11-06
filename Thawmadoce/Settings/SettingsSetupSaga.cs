using System;
using System.Configuration;
using MemBus;
using MemBus.Subscribing;
using Thawmadoce.Extensibility;
using Thawmadoce.Frame.Extensions;
using Thawmadoce.Frame.Messaging;
using Thawmadoce.MainApp;

namespace Thawmadoce.Settings
{
    public class SettingsSetupSaga : ISaga, IAcceptDisposeToken
    {
        private readonly IPublisher _publisher;
        private readonly ISettings _settings;
        private IDisposable _disposeToken;

        public SettingsSetupSaga(IPublisher publisher, ISettings settings)
        {
            _publisher = publisher;
            _settings = settings;
        }

        public void Handle(UiSystemReadyUiMsg msg)
        {
            var cfg = GetCfg();
            var x = cfg.AppSettings.Settings["config_dir"];
            if (x == null)
                _publisher.Publish(new ActivateAppDialog(typeof(SettingsViewModel)));
            else
                SetRootToSettings(x.Value);
        }


        public void Handle(SettingsStorageLocationSetTaskMsg msg)
        {
            var cfg = GetCfg();
            cfg.AppSettings.Settings.Add("config_dir", msg.SettingsDirectory);
            cfg.Save(ConfigurationSaveMode.Modified);
            SetRootToSettings(msg.SettingsDirectory);
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

        private void SetRootToSettings(string value)
        {
            _settings.As<ISettingsInitializer>(i => i.SetRoot(value));
        }
    }
}