using System;

namespace Thawmadoce.MainApp
{
    public class ExceptionMsg
    {
        public Exception Exception { get; private set; }

        public ExceptionMsg(Exception exception)
        {
            Exception = exception;
        }
    }
}