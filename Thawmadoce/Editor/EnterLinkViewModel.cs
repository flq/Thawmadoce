using System;
using System.Windows;
using Caliburn.Micro;
using Thawmadoce.Frame;

namespace Thawmadoce.Editor
{
    public class LinkArgs
    {
        public string Link { get; set; }
        public bool UserCanceled { get; set; }
    }

    public class EnterLinkViewModel : AbstractViewModel, IHaveDisplayName
    {
        private readonly LinkArgs _args;

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
            _args.UserCanceled = true;
        }

        public void OK()
        {
            Deactivate(true);
        }

        public string DisplayName { get; set; }

        private static string GetLink()
        {
            var txt = Clipboard.GetText();
            if (!string.IsNullOrEmpty(txt) && (txt.StartsWith("http://") || txt.StartsWith("https://")) && (txt.IndexOf(Environment.NewLine) == -1))
                return txt;
            return null;
        }
    }
}