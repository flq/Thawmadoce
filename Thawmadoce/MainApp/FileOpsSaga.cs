using System.IO;
using MemBus;
using Microsoft.Win32;
using Thawmadoce.Editor;
using Thawmadoce.Extensibility;

namespace Thawmadoce.MainApp
{
    public class FileOpsSaga : ISaga
    {
        private readonly IPublisher _publisher;
        private string _currentSaveFile;
        private string _lastCapturedMarkdown;
        private readonly object _fileLock = new object();

        public FileOpsSaga(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public void Handle(OpenFileUiMsg msg)
        {
            var dlg = new OpenFileDialog { DefaultExt = ".md", Filter = "Markdown (*.md)|*.md" };
            var result = dlg.ShowDialog();

            if (result == false || !File.Exists(dlg.FileName))
                return;
            _currentSaveFile = dlg.FileName;
            _lastCapturedMarkdown = OpenFile(dlg.FileName);
            _publisher.Publish(new NewContentForEditorUiMsg(_lastCapturedMarkdown));
        }

        public void Handle(SaveFileUiMsg msg)
        {
            var dlg = new SaveFileDialog { DefaultExt = ".md", Filter = "Markdown (*.md)|*.md" };
            var result = dlg.ShowDialog();

            if (result == true)
            {
                _currentSaveFile = dlg.FileName;
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
            if (_currentSaveFile != null)
                SaveFile();
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