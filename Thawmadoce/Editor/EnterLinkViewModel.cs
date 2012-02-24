using System;
using System.Windows;
using Caliburn.Micro;
using Scal;
using Thawmadoce.Extensibility;
using Thawmadoce.Frame;

namespace Thawmadoce.Editor
{
    public class LinkArgs : IUserCancelled
    {
        public string Link { get; set; }
        public bool UserCancelled { get; set; }
    }

    public class EnterLinkViewModel : AbstractViewModel, IHaveDisplayName, INeedRemoteControl
    {
        private readonly LinkArgs _args;
        private IDialogRemoteControl _remoteControl;

        public EnterLinkViewModel(LinkArgs args)
        {
            DisplayName = "Enter a Link address...";
            var link = GetLink();
            _args = args;
            if (link != null)
                _args.Link = link;
        }

        public string Link
        {
            get { return _args.Link; }
            set { _args.Link = value; }
        }

        public void Cancel()
        {
            _args.UserCancelled = true;
        }

        public void OK()
        {
            _remoteControl.CloseDialog();
        }

        public string DisplayName { get; set; }

        void INeedRemoteControl.Accept(IDialogRemoteControl remoteControl)
        {
            _remoteControl = remoteControl;
        }

        private static string GetLink()
        {
            var txt = Clipboard.GetText();
            if (!string.IsNullOrEmpty(txt) && (txt.StartsWith("http://") || txt.StartsWith("https://")) && (txt.IndexOf(Environment.NewLine) == -1))
                return txt;
            return null;
        }
    }
}