using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Navigation;
using Thawmadoce.Frame;
using Thawmadoce.Frame.Extensions;
using System.Reactive.Linq;

namespace Thawmadoce.Editor
{
    public class PreviewViewModel : AbstractViewModel
    {
        private WebBrowser _browser;
        private bool _browserCompletedLoad = true;

        public PreviewViewModel(IObservable<NewHtmlMsg> newHtmlMessages, IDispatchServices svc)
        {
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
            browserUi
                .IfNotNull(browser => browser as WebBrowser)
                .IfNotNull(browser =>
                               {
                                   browser.LoadCompleted += HandleBrowserLoadCompleted;
                                   _browser = browser;
                               });
        }
    }
}