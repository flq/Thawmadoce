using Thawmadoce.Frame;

namespace Thawmadoce.MainApp
{
    public class AppItemsViewModel : AbstractViewModel
    {
        private bool _isAppMenuOpen;

        public bool IsAppMenuOpen
        {
            get { return _isAppMenuOpen; }
            set
            {
                _isAppMenuOpen = value;
                NotifyOfPropertyChange(()=>IsAppMenuOpen);
            }
        }

        public void ToggleAppMenu()
        {
            IsAppMenuOpen = !IsAppMenuOpen;
        }

        public void Handle(ToggleAppMenuUiMsg msg)
        {
            ToggleAppMenu();
        }
    }
}