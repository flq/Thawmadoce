using System;
using System.Diagnostics;
using System.Windows.Input;
using MarkdownSharp;
using MemBus;
using Thawmadoce.Frame;
using Thawmadoce.Frame.Extensions;

namespace Thawmadoce.Editor
{
    public class MarkdownEditorViewModel : AbstractViewModel
    {

        private readonly IPublisher _publisher;
        private readonly Func<IGestureService> _gestureSvc;
        private readonly Markdown _markdown = new Markdown();
        private string _lastMarkdownText;

        public MarkdownInputViewModel Editor { get; set; }
        public PreviewViewModel Preview { get; set; }

        public bool EditorVisible { get; private set; }
        public bool PreviewVisible { get; private set; }

        public MarkdownEditorViewModel(IPublisher publisher, Func<IGestureService> gestureSvc)
        {
            _publisher = publisher;
            _gestureSvc = gestureSvc;
            EditorVisible = true;
        }

        public void Handle(NewMarkdownTaskMsg msg)
        {
            _lastMarkdownText = msg.MarkdownText;
            var html = _markdown.Transform(_lastMarkdownText);
            _publisher.Publish(new NewHtmlMsg(html));
        }

        protected override void OnActivate()
        {
            this.ActivateAllChilds();
            _gestureSvc().AddKeyBinding(new KeyBinding(new RelayCommand(SwitchEditorAndPreview), Key.Tab, ModifierKeys.Control), this);
        }

        private void SwitchEditorAndPreview()
        {
            EditorVisible = !EditorVisible;
            PreviewVisible = !PreviewVisible;
            NotifyOfPropertyChange(()=>EditorVisible);
            NotifyOfPropertyChange(()=>PreviewVisible);
        }
    }
}