using System;
using Caliburn.Micro;
using Scal;
using Thawmadoce.Extensibility;
using Thawmadoce.Frame;
using Thawmadoce.Frame.Messaging;

namespace Thawmadoce.MainApp
{
    public class AlertMsg : ActivateAppDialog
    {
        public AlertMsg(AlertType alertType, string title, string body) : base(typeof(AlertViewModel))
        {
            AlertType = alertType;
            Title = title;
            Body = body;
        }

        public AlertType AlertType { get; private set; }
        public string Title { get; private set; }

        public string Body { get; private set; }
    }

    public interface IHaveAlertType
    {
        AlertType AlertType { get; }
    }

    public enum AlertType
    {
        Information,
        Warning,
        Error
    }

    public class AlertViewModel : AbstractViewModel, IHasTitle, IHaveAlertType
    {
        public AlertViewModel(ActivateViewModelMsg input)
        {
            var alert = (AlertMsg)input;
            Title = alert.Title;
            AlertType = alert.AlertType;
            Body = alert.Body;
        }

        public string Body { get; private set; }

        public AlertType AlertType { get; private set; }

        public string Title
        {
            get;
            private set;
        }
    }
}