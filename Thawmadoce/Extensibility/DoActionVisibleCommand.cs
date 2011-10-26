using System;

namespace Thawmadoce.Extensibility
{
    public class DoActionVisibleCommand : VisibleCommand, IExecutedCallback
    {
        private readonly Action _action;
        private Action _executedCallback;

        public DoActionVisibleCommand(Action action)
        {
            _action = action;
        }

        protected sealed override void InternalExecute()
        {
            _action();
            if (_executedCallback != null)
                _executedCallback();
        }

        void IExecutedCallback.SetCallback(Action executedCallback)
        {
            _executedCallback = executedCallback;
        }
    }
}