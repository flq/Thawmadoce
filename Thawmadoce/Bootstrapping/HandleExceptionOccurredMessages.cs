using System;
using MemBus;
using MemBus.Messages;
using Thawmadoce.Frame;
using Thawmadoce.Frame.Extensions;
using Thawmadoce.MainApp;
using System.Reactive.Linq;

namespace Thawmadoce.Bootstrapping
{
    public class HandleExceptionOccurredMessages : IStartupTask
    {
        private readonly IPublisher _publisher;
        private readonly IObservable<ExceptionOccurred> _exceptionStream;

        public HandleExceptionOccurredMessages(
            IPublisher publisher, 
            IDispatchServices dispatch, 
            IObservable<ExceptionOccurred> exceptionStream1,
            IObservable<ExceptionMsg> exceptionStream2)
        {
            _publisher = publisher;
            exceptionStream1.Select(ex => ex.Exception)
            .Merge(exceptionStream2.Select(ex => ex.Exception))
            .ObserveOn(dispatch)
            .Subscribe(HandleNextException);
        }

        public void Run()
        {
            
        }

        private void HandleNextException(Exception x)
        {
            _publisher.Publish(new ActivateExceptionAppDialog(x));
        }
    }
}