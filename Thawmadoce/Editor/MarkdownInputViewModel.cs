using System;
using System.Collections.ObjectModel;
using System.Threading;
using MemBus;
using Thawmadoce.Editor.SelectionCommands;
using Thawmadoce.Extensibility;
using Thawmadoce.Frame;
using System.Linq;
using Thawmadoce.Frame.Extensions;

namespace Thawmadoce.Editor
{
    public class MarkdownInputViewModel : AbstractViewModel
    {
        private readonly IPublisher _publisher;
        private readonly ISelectionPlugin[] _selectionPlugins;
        private readonly ObservableCollection<SelectionCommand> _selectionCommands = new ObservableCollection<SelectionCommand>();
        private string _markdownText;

        private readonly Timer _typingTimer;
        private string _currentSelection;
        private bool _showSelectionBar;

        public MarkdownInputViewModel(IPublisher publisher, ISelectionPlugin[] selectionPlugins)
        {
            _publisher = publisher;
            _selectionPlugins = selectionPlugins;
            _typingTimer = new Timer(TimerCallback);
        }

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

        public ObservableCollection<SelectionCommand> SelectionCommands
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
                SelectionCommands.ClearAndAddRange(_selectionPlugins.SelectMany(p => p.GetCommands(_currentSelection)));
                SelectionCommands.ForEach(
                    cmd => cmd.As<ISelectionCommandWireup>(
                        w => w.AfterModificationCallback(AfterCommandModifiedSelection)
                    )
                );
                ShowSelectionBar = true;
            }
            else
            {
                SelectionCommands.Clear();
                ShowSelectionBar = false;
            }
        }

        private void AfterCommandModifiedSelection(string newText)
        {
            CurrentSelection = newText;
            SelectionCommands.Clear();
            ShowSelectionBar = false;
        }
    }
}