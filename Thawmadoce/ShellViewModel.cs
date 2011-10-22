using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using MemBus;
using Thawmadoce.Example;
using Thawmadoce.Frame;
using Thawmadoce.Frame.Messaging;
using Thawmadoce.Frame.Extensions;

namespace Thawmadoce {
    
    public class ShellViewModel : Conductor<object>, IGestureService
    {
        private readonly IPublisher _publisher;
        private UIElement _view;
        private readonly Dictionary<object,List<InputBinding>> _bindings = new Dictionary<object, List<InputBinding>>();
        private readonly object _globalScope = new object();

        public ShellViewModel(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public void StartExample()
        {
            _publisher.Publish(new ActivateViewModelMsg<ExampleViewModel>());
        }

        protected override void OnActivate()
        {
            this.ActivateAllChilds();
        }

        protected override void OnViewAttached(object view, object context)
        {
            _view = (UIElement)view;
        }

        public void Handle(ActivateViewModelMsg msg)
        {
            ActivateItem(msg.ViewModel);
        }

        void IGestureService.AddKeyBinding(KeyBinding binding, object scope)
        {
            var bindings = GetBindingScope(scope);
            bindings.Add(binding);
            _view.InputBindings.Add(binding);
        }

        void IGestureService.RemoveInputBindings(object scope)
        {
            var bindings = GetBindingScope(scope);
            bindings.ForEach(ib => _view.InputBindings.Remove(ib));
            _bindings.Remove(scope);
        }

        private List<InputBinding> GetBindingScope(object scope)
        {
            if (scope == null)
                scope = _globalScope;
            if (!_bindings.ContainsKey(scope))
                _bindings.Add(scope, new List<InputBinding>());
            return _bindings[scope];
        }
    }
}
