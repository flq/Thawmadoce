using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Threading;
using Caliburn.Micro;
using Thawmadoce.Frame;
using Thawmadoce.Frame.Extensions;

namespace Thawmadoce.MainApp
{
    public class AppDialogAdapterViewModel : IDeactivate, INotifyPropertyChanged
    {
        private readonly IDispatchServices _svc;

        public AppDialogAdapterViewModel(object dialog, IDispatchServices svc)
        {
            _svc = svc;
            Dialog = dialog;
            Close = new RelayCommand(DoClose);
        }

        public object Dialog { get; private set; }

        public ICommand Close { get; private set; }

        private void DoClose()
        {
            Deactivate(true);
        }

        public void Deactivate(bool close)
        {
            Dialog.As<IDeactivate>(d => d.Deactivate(close));
            CloseTrigger = true;
            PropertyChanged.Raise(this, "CloseTrigger");
            //Woa, this stinks, but it is so incredibly painful to get the completed information from the storyboard
            //We just wait the same time and then say we are done
            _svc.DoActionAfterPeriod(TimeSpan.FromMilliseconds(1100), CloseTriggerCompleted);
        }

        public event EventHandler<DeactivationEventArgs> AttemptingDeactivation;
        public event EventHandler<DeactivationEventArgs> Deactivated;

        public bool CloseTrigger
        {
            get;
            set;
        }

        public void CloseTriggerCompleted()
        {
            Deactivated.Raise(this, new DeactivationEventArgs { WasClosed = true});
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}