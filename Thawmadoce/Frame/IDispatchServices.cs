using System;
using System.Threading;
using System.Windows.Threading;

namespace Thawmadoce.Frame
{
    public interface IDispatchServices
    {
        Dispatcher Dispatcher { get; }
        SynchronizationContext SyncContext { get; }
        void EnsureActionOnDispatcher(Action action);
        void DoActionAfterPeriod(TimeSpan period, Action action);
    }
}