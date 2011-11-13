using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using Caliburn.Micro;
using Thawmadoce.Extensibility;
using Thawmadoce.Frame;
using Thawmadoce.Frame.Extensions;

namespace Thawmadoce.MainApp
{
    public class AppDialogAdapterViewModel : IDeactivate, INotifyPropertyChanged
    {
        private readonly IDispatchServices _svc;
        private bool _deactivationFromHere;

        public AppDialogAdapterViewModel(object dialog, IDispatchServices svc)
        {
            _svc = svc;
            Dialog = dialog;
            Dialog.As<IDeactivate>(d => d.Deactivated += HandleDeactivation);
            Dialog.As<IHasTitle>(d => Title = d.Title);
            TrySetTitleColor(Dialog);
            Close = new RelayCommand(DoClose);
        }

        private void HandleDeactivation(object sender, DeactivationEventArgs e)
        {
            sender.As<IDeactivate>(d => d.Deactivated -= HandleDeactivation);
            if (_deactivationFromHere)
                return;
            Deactivate(e.WasClosed);
        }

        public object Dialog { get; private set; }

        public Brush TitleColor { get; private set; }

        public string Title { get; private set; }

        public ICommand Close { get; private set; }

        private void DoClose()
        {
            Deactivate(true);
        }

        public void Deactivate(bool close)
        {
            _deactivationFromHere = true;
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

        private void TrySetTitleColor(object dialog)
        {
            TitleColor = new SolidColorBrush(Colors.Black);
            dialog.As<IHaveAlertType>(at =>
                                          {
                                              switch (at.AlertType)
                                              {
                                                  case AlertType.Information:
                                                      TitleColor = new SolidColorBrush(Colors.Green);
                                                      break;
                                                  case AlertType.Warning:
                                                      TitleColor = new SolidColorBrush(Colors.DarkViolet);
                                                      break;
                                                  case AlertType.Error:
                                                      TitleColor = new SolidColorBrush(Colors.OrangeRed);
                                                      break;
                                              }
                                          });
        }
    }
}