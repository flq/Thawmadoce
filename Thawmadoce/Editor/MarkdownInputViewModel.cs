using System;
using System.Collections.ObjectModel;
using System.Threading;
using MemBus;
using Thawmadoce.Editor.SelectionCommands;
using Thawmadoce.Extensibility;
using Thawmadoce.Frame;
using System.Linq;
using Thawmadoce.Frame.Extensions;
using Thawmadoce.MainApp;

namespace Thawmadoce.Editor
{
    public class MarkdownInputViewModel : AbstractViewModel
    {
        private readonly IPublisher _publisher;
        private readonly Func<IGestureService> _gestureSvcFactory;
        private readonly Timer _typingTimer;
        private object _commandKeysScope;
        private readonly ISelectionPlugin[] _selectionPlugins;

        // XAML-related state
        private readonly ObservableCollection<IVisibleCommand> _selectionCommands = new ObservableCollection<IVisibleCommand>();
        private string _markdownText;
        private string _currentSelection;
        private bool _showSelectionBar;

        public MarkdownInputViewModel(
            IPublisher publisher, 
            Func<IGestureService> gestureSvcFactory,  
            IObservable<IRefocusEditor> refocusEditorStream,
            IObservable<AppendTextUiMsg> appendTextStream,
            ISelectionPlugin[] selectionPlugins)
        {
            _publisher = publisher;
            _gestureSvcFactory = gestureSvcFactory;
            _selectionPlugins = selectionPlugins;
            RefocusEditorStream = refocusEditorStream;
            TextAppendStream = appendTextStream;
            _typingTimer = new Timer(TimerCallback);
        }

        public void Handle(NewContentForEditorUiMsg msg)
        {
            MarkdownText = msg.Text;
        }

        public IObservable<IRefocusEditor> RefocusEditorStream { get; private set; }
        public IObservable<AppendTextUiMsg> TextAppendStream { get; private set; }

        public string MarkdownText
        {
            get { return _markdownText; }
            set
            {
                _typingTimer.Change(300, int.MaxValue);
                _markdownText = value;
                NotifyOfPropertyChange(()=>MarkdownText);
            }
        }

        public string CurrentSelection
        {
            get { return _currentSelection; }
            set
            {
                _currentSelection = value;
                NotifyOfPropertyChange(() => CurrentSelection);
                PopulateAndHandleTheSelectionBar();
            }
        }

        public bool ShowSelectionBar
        {
            get { return _showSelectionBar; }
            private set
            {
                _showSelectionBar = value;
                NotifyOfPropertyChange(()=>ShowSelectionBar);
            }
        }

        public ObservableCollection<IVisibleCommand> SelectionCommands
        {
            get { return _selectionCommands; }
        }

        private void TimerCallback(object state)
        {
            _publisher.Publish(new NewMarkdownTaskMsg(_markdownText));
        }

        private void PopulateAndHandleTheSelectionBar()
        {
            var relevantTextSelected = !string.IsNullOrEmpty(_currentSelection);
            if (relevantTextSelected)
            {
                _commandKeysScope = new object();

                SelectionCommands.ClearAndAddRange(
                    _selectionPlugins
                      .SelectMany(p => p.GetCommands(new TextContext(_currentSelection, _markdownText)))
                      .Pipeline(cmd => cmd.As<ISelectionCommandWireup>(w => w.AfterModificationCallback(AfterCommandModifiedSelection)))
                      .Pipeline(cmd => { if (cmd.HasKeyBinding) _gestureSvcFactory().AddKeyBinding(cmd.KeyBinding, _commandKeysScope); })
                );
                ShowSelectionBar = true;
            }
            else
            {
                SelectionCommands.Clear();
                ShowSelectionBar = false;
            }
        }

        private void AfterCommandModifiedSelection(TextContext newText)
        {
            CurrentSelection = newText.CurrentSelection;
            if (newText.HasTextToAppend)
                _publisher.Publish(new AppendTextUiMsg(newText.TextToAppend));
            _gestureSvcFactory().RemoveInputBindings(_commandKeysScope);
            _commandKeysScope = null;
            SelectionCommands.Clear();
            ShowSelectionBar = false;
        }
    }
}