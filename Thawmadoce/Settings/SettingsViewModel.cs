using System;
using System.IO;
using MemBus;
using Thawmadoce.Frame;

namespace Thawmadoce.Settings
{
    public class SettingsViewModel : AbstractViewModel
    {
        private readonly IPublisher _publisher;
        private bool _appFolder;
        private bool _userFolder;

        public SettingsViewModel(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public bool AppFolder
        {
            get { return _appFolder; }
            set
            {
                _appFolder = value;
                NotifyOfPropertyChange(()=>AppFolder);
            }
        }

        public string AppFolderLocation
        {
            get
            {
                return Directory.GetCurrentDirectory() + "\\.thawmadoce";
            }
        }

        public bool UserFolder
        {
            get { return _userFolder; }
            set
            {
                _userFolder = value;
                NotifyOfPropertyChange(()=>UserFolder);
            }
        }

        public string UserFolderLocation
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\.thawmadoce";
            }
        }

        public void CustomFolder()
        {
            
        }

        public void Done()
        {
            if (AppFolder)
                _publisher.Publish(new SettingsStorageLocationSetTaskMsg(AppFolderLocation));
            else if (UserFolder)
                _publisher.Publish(new SettingsStorageLocationSetTaskMsg(UserFolderLocation));
            Deactivate(true);
        }
    }
}