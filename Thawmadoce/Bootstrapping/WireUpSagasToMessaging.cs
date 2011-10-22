using System;
using MemBus;
using Thawmadoce.Extensibility;

namespace Thawmadoce.Bootstrapping
{
    public class WireUpSagasToMessaging : IStartupTask
    {
        private readonly ISaga[] _sagas;
        private readonly ISubscriber _subscriber;

        public WireUpSagasToMessaging(ISaga[] sagas, ISubscriber subscriber)
        {
            _sagas = sagas;
            _subscriber = subscriber;
        }

        public void Run()
        {
            foreach (var s in _sagas)
                _subscriber.Subscribe(s);
        }
    }
}