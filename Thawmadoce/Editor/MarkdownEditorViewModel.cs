using System;
using MemBus;
using Scal;
using Thawmadoce.Frame.Extensions;

namespace Thawmadoce.Editor
{
    public class MarkdownEditorViewModel : AbstractViewModel
    {

        private readonly IPublisher _publisher;
        private string _lastMarkdownText;

        public MarkdownInputViewModel Editor { get; set; }
        public PreviewViewModel Preview { get; set; }

        public bool EditorVisible { get; private set; }
        public bool PreviewVisible { get; private set; }

        public MarkdownEditorViewModel(IPublisher publisher)
        {
            _publisher = publisher;
            EditorVisible = true;
        }

        public void Handle(NewMarkdownTaskMsg msg)
        {
            _lastMarkdownText = msg.MarkdownText;
            _publisher.Publish(new NewHtmlMsg(_lastMarkdownText.ToHtml()));
        }

        protected override void OnActivate()
        {
            this.ActivateAllChilds();
        }

        public void Handle(TogglePreviewUiMsg msg)
        {
            EditorVisible = !EditorVisible;
            PreviewVisible = !PreviewVisible;
            NotifyOfPropertyChange(()=>EditorVisible);
            NotifyOfPropertyChange(()=>PreviewVisible);
        }
    }
}