using Thawmadoce.Frame;

namespace Thawmadoce.Settings
{
    public class SettingsViewModel : AbstractViewModel
    {
        public SettingsViewModel()
        {
            
        }

        public void Click()
        {
            Deactivate(true);
        }
    }
}