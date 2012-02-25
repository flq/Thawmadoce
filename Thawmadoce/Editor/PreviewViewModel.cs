using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using DynamicXaml.MarkupSystem;
using MemBus;
using Scal;
using Scal.Services;
using Thawmadoce.Frame;
using Thawmadoce.Frame.Extensions;
using System.Reactive.Linq;
using DynamicXaml.Extensions;

namespace Thawmadoce.Editor
{
    public class PreviewViewModel : AbstractViewModel
    {
        private readonly IPublisher _publisher;
        private WebBrowser _browser;
        private bool _browserCompletedLoad = true;

        public PreviewViewModel(IObservable<NewHtmlMsg> newHtmlMessages, IDispatchServices svc, IPublisher publisher)
        {
            _publisher = publisher;
            newHtmlMessages
                .Where(msg => _browserCompletedLoad)
                .ObserveOn(svc)
                .Subscribe(NextHtml);
        }

        private void NextHtml(NewHtmlMsg msg)
        {
            if (_browser == null)
            {
                Debug.WriteLine("Browser has not been set?!");
                return;
            }

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(msg.Html));
            _browserCompletedLoad = false;
            _browser.NavigateToStream(ms);

        }

        private void HandleBrowserLoadCompleted(object sender, NavigationEventArgs e)
        {
            _browserCompletedLoad = true;
        }

        public void BrowserLoaded(object browserUi)
        {
            browserUi.ToMaybeOf<WebBrowser>()
                .Do(browser =>
                {
                    browser.LoadCompleted += HandleBrowserLoadCompleted;
                    _browser = browser;
                    // HACK: when browser has focus, input bindings of the window are not reached
                    // This is the preview/edit toggle command...
                    _browser.InputBindings.Add(CreateEditpreviewToggleBinding());
                });
        }

        private KeyBinding CreateEditpreviewToggleBinding()
        {
            var cmd = EditorKeyShortcuts.KeyComboToTogglePreview;
            return new KeyBinding(new RelayCommand(() => _publisher.Publish(cmd.MessageFactory())), cmd.Keys.Key, cmd.Keys.Modifier);
        }
    }
}