using System;
using System.ComponentModel;
using System.Windows.Input;
using Thawmadoce.Frame;
using Thawmadoce.Frame.Extensions;

namespace Thawmadoce.RfSitesPublishing
{
    public class ServerViewModel : INotifyPropertyChanged
    {
        private bool _canEdit;
        private readonly Lazy<ICommand> _edit;
        private readonly Lazy<ICommand> _save;

        public ServerViewModel()
        {
            _edit = new Lazy<ICommand>(() => new RelayCommand(() => CanEdit = true));
            _save = new Lazy<ICommand>(() => new RelayCommand(DoSave));
        }

        public bool CanEdit
        {
            get { return _canEdit; }
            set
            {
                _canEdit = value;
                PropertyChanged.Raise(this, _ => _.CanEdit);
            }
        }

        public ICommand Edit
        {
            get { return _edit.Value; }
        }

        public ICommand Save
        {
            get { return _save.Value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler Saved;

        private void DoSave()
        {
            CanEdit = false;
            Saved.Raise(this);
        }
    }
}