using System;
using Thawmadoce.Extensibility;
using Thawmadoce.Frame;

namespace Thawmadoce.RfSitesPublishing
{
    public class EnterIdArgs : IUserCancelled
    {
        public string Id { get; set; }
        public string Server { get; set; }
        public string Token { get; set; }
        public bool UserCancelled { get; set; }
    }

    public class EnterIdViewModel : INeedRemoteControl
    {
        private readonly EnterIdArgs _args;
        private IDialogRemoteControl _remoteControl;

        public EnterIdViewModel(EnterIdArgs args)
        {
            _args = args;
        }

        public EnterIdArgs Args
        {
            get { return _args; }
        }

        public void Load()
        {
            _remoteControl.CloseDialog();
        }

        public void Accept(IDialogRemoteControl remoteControl)
        {
            _remoteControl = remoteControl;
            remoteControl.SetDialogTitle("Enter id to load the content.");
        }
    }
}