using System;
using System.Collections.Generic;
using System.Windows.Input;
using Thawmadoce.Extensibility;
using Thawmadoce.Frame;
using System.Linq;
using Thawmadoce.Frame.Extensions;

namespace Thawmadoce.MainApp
{
    public class AppItemsViewModel : AbstractViewModel
    {
        private readonly Func<IGestureService> _svc;
        private bool _isAppMenuOpen;
        private readonly List<List<IVisibleCommand>> _appItemsGroups = new List<List<IVisibleCommand>>();

        private readonly List<KeyBinding> _keyBindings = new List<KeyBinding>();
        private readonly object _appMenuScope = new object();


        public AppItemsViewModel(IAppItemsPlugin[] plugins, Func<IGestureService> svc)
        {
            _svc = svc;
            _appItemsGroups = plugins.Select(CreateGroup).ToList();
        }

        public bool IsAppMenuOpen
        {
            get { return _isAppMenuOpen; }
            set
            {
                _isAppMenuOpen = value;
                NotifyOfPropertyChange(()=>IsAppMenuOpen);
            }
        }

        public IEnumerable<IEnumerable<IVisibleCommand>> AppItemsGroups { get { return _appItemsGroups; } }

        public void ToggleAppMenu()
        {
            if (!IsAppMenuOpen)
                RegisterKeyBindings();
            if (IsAppMenuOpen)
                RemoveBindings();
            IsAppMenuOpen = !IsAppMenuOpen;
        }

        private void RemoveBindings()
        {
            _svc().RemoveInputBindings(_appMenuScope);
        }

        private void RegisterKeyBindings()
        {
            var svc = _svc();
            _keyBindings.ForEach(kb => svc.AddKeyBinding(kb, _appMenuScope));
        }

        public void Handle(ToggleAppMenuUiMsg msg)
        {
            ToggleAppMenu();
        }

        private List<IVisibleCommand> CreateGroup(IAppItemsPlugin plugin)
        {
            return plugin.GetCommands()
                .Pipeline(cmd => cmd.As<IExecutedCallback>(c => c.SetCallback(ToggleAppMenu)))
                .Pipeline(cmd => { if (cmd.HasKeyBinding) _keyBindings.Add(cmd.KeyBinding); })
                .ToList();
        }
    }
}