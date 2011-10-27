using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using MemBus;
using Thawmadoce.Editor;
using Thawmadoce.Extensibility;
using Thawmadoce.Frame;
using Thawmadoce.Frame.Extensions;
using Thawmadoce.MainApp;
using System.Linq;

namespace Thawmadoce {
    
    public class ShellViewModel : Screen
    {
        private readonly IKeyboardOnlyActions[] _keyboardOnlyActions;
        private readonly IPublisher _publisher;
        private readonly GestureService _gestureSvc = new GestureService();

        private string _title = "The awesome markdown centrifuge";

        public MarkdownEditorViewModel Editor { get; set; }
        public AppItemsViewModel AppItems { get; set; }
        public AppDialogsViewModel AppDialogs { get; set; }

        public ShellViewModel(IKeyboardOnlyActions[] keyboardOnlyActions, IPublisher publisher)
        {
            _keyboardOnlyActions = keyboardOnlyActions;
            _publisher = publisher;
        }

        public IGestureService GestureService
        {
            get { return _gestureSvc; }
        }


        protected override void OnActivate()
        {
            this.ActivateAllChilds();
            DisplayName = _title;
        }

        protected override void OnViewAttached(object view, object context)
        {
            _gestureSvc.AttachView((UIElement)view);
            _keyboardOnlyActions
                .SelectMany(kb => kb.GetKeyboardActions())
                .Select(kc => new KeyBinding(new RelayCommand(() => _publisher.Publish(kc.MessageFactory())), kc.Keys.Key, kc.Keys.Modifier))
                .ForEach(kb => _gestureSvc.As<IGestureService>(gsv => gsv.AddKeyBinding(kb)));
        }
    }
}
