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
    }
}