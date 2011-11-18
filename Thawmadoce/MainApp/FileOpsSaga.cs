using System;
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
        private readonly FileSaving _saveFile = new FileSaving();
        private string _lastCapturedMarkdown;
        

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
            LoadContentsAndNotifySystems(dlg.FileName);
        }

        public void Handle(SaveFileUiMsg msg)
        {
            var dlg = new SaveFileDialog { DefaultExt = ".md", Filter = FileFilter };
            SetFilenameIfAvailable(dlg);
            var result = dlg.ShowDialog();

            if (result == true)
            {
                _saveFile.SetSaveFile(dlg.FileName);
                SaveText();
                _settings.Delete("Core.TmpText");
            }
        }

        public void Handle(NewFileMsg msg)
        {
            _saveFile.Reset();
            _lastCapturedMarkdown = null;
            _publisher.Publish(new NewContentForEditorUiMsg());
            _publisher.Publish(new NewDisplayNameUiMsg());
        }

        public void Handle(NewMarkdownTaskMsg msg)
        {
            _lastCapturedMarkdown = msg.MarkdownText;
            SaveText();
        }

        private void LoadContentsAndNotifySystems(string fileName)
        {
            _saveFile.SetSaveFile(fileName);
            _lastCapturedMarkdown = _saveFile.LoadFile();
            _publisher.Publish(new NewContentForEditorUiMsg(_lastCapturedMarkdown));
            _publisher.Publish(new NewDisplayNameUiMsg(_saveFile.FileName));
        }

        private void SaveText()
        {
            if (!_saveFile.SaveFile(_lastCapturedMarkdown ?? ""))
                _settings.Post("Core.TmpText", _lastCapturedMarkdown);
        }

        private void SetFilenameIfAvailable(FileDialog dlg)
        {
            if (_saveFile.IsSaveFileDefined)
                dlg.FileName = _saveFile.FileName;
        }

        private class FileSaving
        {
            private readonly object _fileLock = new object();
            private string _currentSaveFile;

            public bool IsSaveFileDefined { get { return _currentSaveFile != null; } }
            public string FileName { get { return Path.GetFileName(_currentSaveFile); } }

            public void SetSaveFile(string fileName)
            {
                _currentSaveFile = fileName;
            }

            public string LoadFile()
            {
                if (_currentSaveFile == null)
                    throw new InvalidOperationException("No save file location known!");
                using (var sr = File.OpenText(_currentSaveFile))
                {
                    return sr.ReadToEnd();
                }
            }

            public bool SaveFile(string contents)
            {
                if (_currentSaveFile == null)
                    return false;
                lock (_fileLock)
                {
                    using (var fs = new FileStream(_currentSaveFile, FileMode.Create))
                    using (var sw = new StreamWriter(fs))
                    {
                        sw.Write(contents);
                    }
                    using (var fs = new FileStream(HtmlFile(), FileMode.Create))
                    using (var sw = new StreamWriter(fs))
                    {
                        sw.Write(contents.ToHtml());
                    }
                }
                return true;
            }

            public void Reset()
            {
                _currentSaveFile = null;
            }

            private string PathToFile { get { return Path.GetDirectoryName(_currentSaveFile); } }

            private string HtmlFile()
            {
                var file = Path.GetFileNameWithoutExtension(_currentSaveFile);
                return Path.Combine(PathToFile, file + ".html");
            }
        }
    }
}