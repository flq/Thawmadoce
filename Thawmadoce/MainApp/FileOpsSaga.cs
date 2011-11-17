using System.IO;
using MemBus;
using Microsoft.Win32;
using Thawmadoce.Editor;
using Thawmadoce.Extensibility;
using Thawmadoce.Frame.Messaging;
using Thawmadoce.Settings;

namespace Thawmadoce.MainApp
{
    public class FileOpsSaga : ISaga
    {
        private const string FileFilter = "Markdown (*.md)|*.md|Text files (*.txt)|*.txt|All Files (*.*)|*.*";

        private readonly IPublisher _publisher;
        private readonly ISettings _settings;
        private string _currentSaveFile;
        private string _lastCapturedMarkdown;
        private readonly object _fileLock = new object();

        public FileOpsSaga(IPublisher publisher, ISettings settings)
        {
            _publisher = publisher;
            _settings = settings;
        }

        public void Handle(UiSystemReadyUiMsg msg)
        {
            var s = _settings.Get<string>("Core.TmpText");
            if (s != null)
                _lastCapturedMarkdown = s;
            _publisher.Publish(new NewContentForEditorUiMsg(_lastCapturedMarkdown));
        }

        public void Handle(OpenFileUiMsg msg)
        {
            var dlg = new OpenFileDialog { DefaultExt = ".md", Filter = FileFilter };
            var result = dlg.ShowDialog();

            if (result == false || !File.Exists(dlg.FileName))
                return;
            _currentSaveFile = dlg.FileName;
            _lastCapturedMarkdown = OpenFile(dlg.FileName);
            _publisher.Publish(new NewContentForEditorUiMsg(_lastCapturedMarkdown));
        }

        public void Handle(SaveFileUiMsg msg)
        {
            var dlg = new SaveFileDialog { DefaultExt = ".md", Filter = FileFilter };
            var result = dlg.ShowDialog();

            if (result == true)
            {
                _currentSaveFile = dlg.FileName;
                _settings.Delete("Core.TmpText");
                SaveFile();
            }
        }

        public void Handle(NewFileMsg msg)
        {
            _currentSaveFile = null;
            _lastCapturedMarkdown = null;

            _publisher.Publish(new NewContentForEditorUiMsg());
        }

        public void Handle(NewMarkdownTaskMsg msg)
        {
            _lastCapturedMarkdown = msg.MarkdownText;
            SaveText();
        }

        private void SaveText()
        {
            if (_currentSaveFile != null)
                SaveFile();
            else
                _settings.Post("Core.TmpText", _lastCapturedMarkdown);
        }

        private void SaveFile()
        {
            lock(_fileLock)
            {
                using ( var fs = new FileStream(_currentSaveFile, FileMode.Create))
                using (var sw = new StreamWriter(fs))
                {
                    sw.Write(_lastCapturedMarkdown);
                }
            }
        }

        private static string OpenFile(string fileName)
        {
            using (var sr = File.OpenText(fileName))
            {
                return sr.ReadToEnd();
            }
            
        }
    }
}