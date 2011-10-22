using System;
using System.Threading;
using MemBus;
using Thawmadoce.Frame;

namespace Thawmadoce.Editor
{
    public class MarkdownInputViewModel : AbstractViewModel
    {
        private readonly IPublisher _publisher;
        private string _markdownText;

        private readonly Timer _timer;

        public MarkdownInputViewModel(IPublisher publisher)
        {
            _publisher = publisher;
            _timer = new Timer(TimerCallback);
        }

        public string MarkdownText
        {
            get { return _markdownText; }
            set
            {
                _timer.Change(1000, int.MaxValue);
                _markdownText = value;
                NotifyOfPropertyChange(()=>MarkdownText);
            }
        }

        private void TimerCallback(object state)
        {
            _publisher.Publish(new NewMarkdownTaskMsg(_markdownText));
        }
    }
}