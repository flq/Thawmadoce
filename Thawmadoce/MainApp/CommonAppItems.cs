using System.Collections.Generic;
using System.Windows.Input;
using MemBus;
using Thawmadoce.Extensibility;

namespace Thawmadoce.MainApp
{
    public class CommonAppItems : IAppItemsPlugin
    {
        private readonly IPublisher _publisher;

        public CommonAppItems(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public IEnumerable<IVisibleCommand> GetCommands()
        {
            yield return new DoActionVisibleCommand(() => _publisher.Publish(new NewFileMsg()))
                             {
                                 CommandIcon = "/Thawmadoce;component/Media/document-new.png",
                                 CommandText = "New Text",
                                 KeyCombination = new KeyCombo(Key.N, ModifierKeys.Control)
                             };

            yield return new DoActionVisibleCommand(() => _publisher.Publish(new OpenFileUiMsg()))
                             {
                                 CommandIcon = "/Thawmadoce;component/Media/document_open.png",
                                 CommandText = "Open a Text",
                                 KeyCombination = new KeyCombo(Key.O, ModifierKeys.Control)
                             };
            yield return new DoActionVisibleCommand(() => _publisher.Publish(new SaveFileUiMsg()))
                             {
                                 CommandIcon = "/Thawmadoce;component/Media/document_save.png",
                                 CommandText = "Save current text",
                                 KeyCombination = new KeyCombo(Key.S, ModifierKeys.Control)
                             };

        }
    }
}