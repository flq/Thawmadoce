using System;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using MemBus;
using Thawmadoce.Editor;
using Thawmadoce.Extensibility;
using Thawmadoce.Frame;
using Thawmadoce.Frame.Extensions;
using Thawmadoce.Frame.Messaging;
using Thawmadoce.MainApp;
using System.Linq;

namespace Thawmadoce {
    
    public class ShellViewModel : Screen
    {
        private readonly IKeyboardOnlyActions[] _keyboardOnlyActions;
        private readonly IPublisher _publisher;
        private readonly GestureService _gestureSvc = new GestureService();

        private const string Title = "The awesome markdown centrifuge";

        public WindowHeaderViewModel WindowHeader { get; set; }
        public MarkdownEditorViewModel Editor { get; set; }
        public AppItemsViewModel AppItems { get; set; }
        public AppDialogsViewModel AppDialogs { get; set; }

        public ShellViewModel(IKeyboardOnlyActions[] keyboardOnlyActions, IPublisher publisher)
        {
            _keyboardOnlyActions = keyboardOnlyActions;
            _publisher = publisher;
        }

        public void Handle(NewDisplayNameUiMsg msg)
        {
            if (msg.IsTitleReset)
                DisplayName = "Thawmadoce";
            DisplayName = (msg.Append ? "Thawmadoce - " : "") + msg.NewTitle;
            //NotifyOfPropertyChange(()=>DisplayName);
        }

        public IGestureService GestureService
        {
            get { return _gestureSvc; }
        }


        protected override void OnActivate()
        {
            this.ActivateAllChilds();
            DisplayName = Title;
        }

        protected override void OnViewAttached(object view, object context)
        {
            var uiElement = (UIElement)view;
            _gestureSvc.AttachView(uiElement);
            _keyboardOnlyActions
                .SelectMany(kb => kb.GetKeyboardActions())
                .Select(kc => new KeyBinding(new RelayCommand(() => _publisher.Publish(kc.MessageFactory())), kc.Keys.Key, kc.Keys.Modifier))
                .ForEach(kb => _gestureSvc.As<IGestureService>(gsv => gsv.AddKeyBinding(kb)));
            view.As<FrameworkElement>(vw => vw.Loaded += HandleViewLoaded);
        }

        private void HandleViewLoaded(object sender, RoutedEventArgs e)
        {
            sender.As<FrameworkElement>(vw => vw.Loaded -= HandleViewLoaded);
            _publisher.Publish(new UiSystemReadyUiMsg());
        }
    }
}
