using System;
using System.Threading;
using System.Windows.Threading;

namespace Thawmadoce.Frame
{
    public class WpfDispatchService : IDispatchServices
    {
        public WpfDispatchService(DispatcherObject app)
        {
            Dispatcher = app.Dispatcher;
            SyncContext = new DispatcherSynchronizationContext(Dispatcher);
        }

        public Dispatcher Dispatcher { get; private set; }

        public SynchronizationContext SyncContext { get; private set; }

        public void EnsureActionOnDispatcher(Action action)
        {
            var isOnThread = Dispatcher.CheckAccess();
            if (isOnThread)
                action();
            else
                Dispatcher.Invoke(action);
        }
    } 
}