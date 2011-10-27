using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Threading;

namespace Thawmadoce.Frame
{
    public class WpfDispatchService : IDispatchServices
    {
        private Dictionary<DispatcherTimer,Action> timers = new Dictionary<DispatcherTimer, Action>();
        private Action nextDispatcherAction;

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

        public void DoActionAfterPeriod(TimeSpan period, Action action)
        {
            var t = new DispatcherTimer(DispatcherPriority.Normal, Dispatcher);
            t.Tick += HandleTick;
            t.Interval = period;
            timers.Add(t, action);
            t.Start();
        }

        private void HandleTick(object sender, EventArgs e)
        {
            var t = (DispatcherTimer)sender;
            t.Stop();
            timers[t]();
            timers.Remove(t);
        }
    } 
}