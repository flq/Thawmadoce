using MarkdownSharp;
using MemBus;
using Thawmadoce.Frame;
using Thawmadoce.Frame.Extensions;

namespace Thawmadoce.Editor
{
    public class MarkdownEditorViewModel : AbstractViewModel
    {

        private readonly IPublisher _publisher;
        private readonly Markdown _markdown = new Markdown();
        private string _lastMarkdownText;

        public MarkdownInputViewModel Editor { get; set; }
        public PreviewViewModel Preview { get; set; }

        public MarkdownEditorViewModel(IPublisher publisher)
        {
            _publisher = publisher;
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
        }
    }
}