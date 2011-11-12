using System;
using System.Collections.Generic;
using Thawmadoce.Frame;
using Thawmadoce.Frame.Messaging;

namespace Thawmadoce.MainApp
{
    public class ExceptionDialogViewModel : AbstractViewModel
    {
        private readonly List<List<ExceptionViewModel>> _exceptions = new List<List<ExceptionViewModel>>();

        public ExceptionDialogViewModel(ActivateViewModelMsg input)
        {
            var exception = ((ActivateExceptionAppDialog)input).Exception;

            if (exception is AggregateException)
            {
                foreach (var x in ((AggregateException)exception).InnerExceptions)
                    _exceptions.Add(FlattenException(x));
            }
            else
            {
                _exceptions.Add(FlattenException(exception));
            }
        }

        private static List<ExceptionViewModel> FlattenException(Exception exception)
        {
            var l = new List<ExceptionViewModel>();
            var x = exception;
            while (x != null)
            {
                l.Add(new ExceptionViewModel(exception));
                x = x.InnerException;
            }
            return l;
        }

        public List<List<ExceptionViewModel>> Exceptions { get { return _exceptions; } }

        
        public class ExceptionViewModel
        {
            private readonly Exception _exception;

            public ExceptionViewModel(Exception exception)
            {
                _exception = exception;
            }

            public string Type { get { return _exception.GetType().Name; } }

            public string Message { get { return _exception.Message; } }

            public string StackTrace { get { return _exception.StackTrace; } }
        }

    }
}