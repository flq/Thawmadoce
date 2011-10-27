using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using MemBus;
using Thawmadoce.Editor;
using Thawmadoce.Extensibility;
using Thawmadoce.Frame.Messaging;
using Thawmadoce.Settings;

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
            yield return new DoActionVisibleCommand(SendInfo)
                             {
                                 CommandIcon = "/Thawmadoce;component/Media/info.png",
                                 CommandText = "Some info on Thawmadoce",
                             };
            yield return new DoActionVisibleCommand(()=> _publisher.Publish(new ActivateAppDialog(typeof(SettingsViewModel))))
                             {
                                 CommandText = "Test",
                             };

        }

        private void SendInfo()
        {
            _publisher.Publish(new NewContentForEditorUiMsg(LoadFile()));
        }

        private static Stream GetStream()
        {
            return typeof(ShellView).Assembly
              .GetManifestResourceStream(typeof(ShellView), "help.md");
        }

        private static string LoadFile()
        {
            Stream stream = GetStream();
            var sr = new StreamReader(stream);
            return sr.ReadToEnd();
        }
    }
}