using Caliburn.Micro;
using Thawmadoce.MainApp;

namespace Thawmadoce
{
    public class WindowHeaderViewModel : PropertyChangedBase
    {
        private const string title = "The awesome markdown centrifuge";

        public WindowHeaderViewModel()
        {
            Title = title;
        }

        public void Handle(NewDisplayNameUiMsg msg)
        {
            if (msg.IsTitleReset)
                Title = "Thawmadoce";
            Title = (msg.Append ? "Thawmadoce - " : "") + msg.NewTitle;
            NotifyOfPropertyChange(()=>Title);
        }

        public string Title { get; set; }
    }
}