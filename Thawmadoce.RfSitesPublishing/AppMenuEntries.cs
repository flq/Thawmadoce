using System;
using System.Collections.Generic;
using System.Windows.Input;
using Thawmadoce.Extensibility;
using Thawmadoce.Frame.Messaging;
using Thawmadoce.MainApp;

namespace Thawmadoce.RfSitesPublishing
{
    public class AppMenuEntries : IAppItemsPlugin
    {
        private readonly IMessagePublisher _publisher;

        public AppMenuEntries(IMessagePublisher publisher)
        {
            _publisher = publisher;
        }

        public IEnumerable<IVisibleCommand> GetCommands()
        {
            yield return new DoActionVisibleCommand(() => _publisher.Publish(new ActivateAppDialog(typeof(PublishingViewModel))))
            {
                CommandIcon = "/Thawmadoce.RfSitesPublishing;component/Media/archive_extract.png",
                CommandText = "Rf.Sites publishing",
                KeyCombination = new KeyCombo(Key.P, ModifierKeys.Control)
            };
        }
    }
}