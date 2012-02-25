using System;
using MemBus;
using MemBus.Messages;
using Scal.Bootstrapping;
using Scal.Configuration;
using Scal.Services;
using Thawmadoce.MainApp;
using System.Reactive.Linq;
using Scal;

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

        public TaskPriority Priority
        {
            get { return TaskPriority.Later; }
        }

        private void HandleNextException(Exception x)
        {
            _publisher.Publish(new ActivateExceptionAppDialog(x));
        }

        public class UnhandledExceptionHandler : IExceptionHandler
        {
            private readonly IPublisher _publisher;

            public UnhandledExceptionHandler(IPublisher publisher)
            {
                _publisher = publisher;
            }

            public bool ShouldTerminateApp(Exception x)
            {
                _publisher.Publish(new ExceptionOccurred(x));
                return false;
            }
        }
    }
}