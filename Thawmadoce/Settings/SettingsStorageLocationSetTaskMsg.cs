namespace Thawmadoce.Settings
{
    public class SettingsStorageLocationSetTaskMsg
    {
        public SettingsStorageLocationSetTaskMsg(string settingsDirectory)
        {
            SettingsDirectory = settingsDirectory;
        }

        public string SettingsDirectory { get; private set; }
    }
}