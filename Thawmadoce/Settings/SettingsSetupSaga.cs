using System;
using System.Configuration;
using MemBus;
using MemBus.Subscribing;
using Thawmadoce.Bootstrapping;
using Thawmadoce.Extensibility;
using Thawmadoce.Frame.Extensions;
using Thawmadoce.Frame.Messaging;
using Thawmadoce.MainApp;

namespace Thawmadoce.Settings
{
    public class SettingsStartup : IStartupTask
    {
        private readonly ISettings _settings;

        public SettingsStartup(ISettings settings)
        {
            _settings = settings;
        }

        public void Run()
        {
            var v = SettingsSetupSaga.GetDirectoryValue();
            if (v != null)
                _settings.As<ISettingsInitializer>(i => i.SetRoot(v.Value));
        }
    }

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
            if (_settings.IsSetUp)
            {
                End();
                return;
            }

            var x = GetDirectoryValue();
            if (x != null)
            {
                SetRootToSettings(x.Value);
                End();
                return;
            }
            _publisher.Publish(new ActivateAppDialog(typeof(SettingsViewModel)));
        }


        public void Handle(SettingsStorageLocationSetTaskMsg msg)
        {
            var cfg = GetCfg();
            cfg.AppSettings.Settings.Add("config_dir", msg.SettingsDirectory);
            cfg.Save(ConfigurationSaveMode.Modified);
            SetRootToSettings(msg.SettingsDirectory);
            _publisher.Publish(new RefocusEditorUiMsg());
            End();
        }

        private void End()
        {
            _disposeToken.Dispose();
        }

        public void Accept(IDisposable disposeToken)
        {
            _disposeToken = disposeToken;
        }

        public static KeyValueConfigurationElement GetDirectoryValue()
        {
            var cfg = GetCfg();
            return cfg.AppSettings.Settings["config_dir"];
        }

        public static Configuration GetCfg()
        {
            return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        private void SetRootToSettings(string value)
        {
            _settings.As<ISettingsInitializer>(i => i.SetRoot(value));
        }
    }
}