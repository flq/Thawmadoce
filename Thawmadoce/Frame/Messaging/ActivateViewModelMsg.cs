using System;
using MemBus.Support;

namespace Thawmadoce.Frame.Messaging
{


    /// <summary>
    /// Activate App Dialog
    /// </summary>
    public class ActivateAppDialog : ActivateViewModelMsg {
        public ActivateAppDialog(Type viewModelType) : base(viewModelType)
        {
        }
    }

    /// <summary>
    /// Generic version
    /// </summary>
    public class ActivateViewModelMsg<T> : ActivateViewModelMsg
    {
        public ActivateViewModelMsg()
            : base(typeof(T))
        {
        }
    }

    /// <summary>
    /// Basic class to request the activation of a view model
    /// </summary>
    public class ActivateViewModelMsg
    {
        public ActivateViewModelMsg(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }

        public Type ViewModelType { get; private set; }
        public object ViewModel { get; private set; }

        public void SetInstantiatedModel(object viewModel)
        {
            if (ModelInstanceAvailable)
                throw new InvalidOperationException("Message already has a viewmodel set of type " + ViewModel.GetType().Name);
            if (!ViewModelType.IsAssignableFrom(viewModel.GetType()))
                throw new InvalidOperationException("Screen {0} is incompatible with {1}".Fmt(viewModel.GetType().Name, ViewModelType.Name));
            ViewModel = viewModel;
        }

        public bool ModelInstanceAvailable
        {
            get { return ViewModel != null; }
        }
    }
}