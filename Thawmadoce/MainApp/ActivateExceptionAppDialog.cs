using System;
using Thawmadoce.Frame.Messaging;

namespace Thawmadoce.MainApp
{
    public class ActivateExceptionAppDialog : ActivateAppDialog
    {
        public ActivateExceptionAppDialog(Exception x) : base(typeof(ExceptionDialogViewModel))
        {
            Exception = x;
        }

        public Exception Exception { get; private set; }
    }
}