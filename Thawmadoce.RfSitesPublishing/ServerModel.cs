using System;
using System.ComponentModel;
using System.Windows.Input;
using DynamicXaml.MarkupSystem;
using Thawmadoce.Frame.Extensions;

namespace Thawmadoce.RfSitesPublishing
{
    public class ServerModel : INotifyPropertyChanged
    {
        private bool _canEdit;
        private readonly Lazy<ICommand> _edit;
        private readonly Lazy<ICommand> _save;

        internal ServerModel(dynamic values) : this()
        {
            Address = values.Address;
            Token = values.Token;
        }

        public ServerModel()
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

        public string Address { get; set; }
        public string Token { get; set; }

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

        public object Compress()
        {
            return new { Address, Token };
        }
    }
}