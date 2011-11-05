using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Caliburn.Micro;
using Thawmadoce.Frame;
using Thawmadoce.Frame.Extensions;
using Thawmadoce.Frame.Messaging;
using System.Linq;

namespace Thawmadoce.MainApp
{
    public class AppDialogsViewModel : AbstractViewModel
    {
        private readonly IDispatchServices _svc;
        private readonly ObservableCollection<AppDialogAdapterViewModel> _appDialogs = new ObservableCollection<AppDialogAdapterViewModel>();

        public AppDialogsViewModel(IDispatchServices svc)
        {
            _svc = svc;
            _appDialogs.CollectionChanged += HandleAppDialogsChange;
        }

        public void Handle(ActivateAppDialog msg)
        {
            var vm = new AppDialogAdapterViewModel(msg.ViewModel, _svc);
            vm.Deactivated += HandleDeactivation;
            _appDialogs.Add(vm);
        }

        private void HandleDeactivation(object sender, DeactivationEventArgs e)
        {
            var model = (AppDialogAdapterViewModel)sender;
            model.Deactivated -= HandleDeactivation;
            _appDialogs.Remove(model);
        }

        public INotifyCollectionChanged AppDialogs { get { return _appDialogs; } }

        public bool AppDialogsVisible { get { return _appDialogs.Count > 0; } }

        private void HandleAppDialogsChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyOfPropertyChange(() => AppDialogsVisible);
        }
    }
};