using System;
using MemBus;
using MemBus.Messages;
using Thawmadoce.Frame;
using Thawmadoce.Frame.Extensions;
using Thawmadoce.MainApp;

namespace Thawmadoce.Bootstrapping
{
    public class HandleExceptionOccurredMessages : IStartupTask
    {
        private readonly IPublisher _publisher;
        private readonly IObservable<ExceptionOccurred> _exceptionStream;

        public HandleExceptionOccurredMessages(IPublisher publisher, IDispatchServices dispatch, IObservable<ExceptionOccurred> exceptionStream)
        {
            _publisher = publisher;
            exceptionStream
                .ObserveOn(dispatch)
                .Subscribe(HandleNextException);
        }

        public void Run()
        {
            
        }

        private void HandleNextException(ExceptionOccurred x)
        {
            _publisher.Publish(new ActivateExceptionAppDialog(x.Exception));
        }
    }
}