using System;
using System.Reactive.Linq;

namespace Thawmadoce.Frame.Extensions
{
    public static class Observable
    {
        public static IObservable<T> ObserveOn<T>(this IObservable<T> observable, IDispatchServices svc)
        {
            return observable.ObserveOn(svc.SyncContext);
        }
        
    }
}