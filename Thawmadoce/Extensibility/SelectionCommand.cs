using System;
using System.Windows.Input;
using Thawmadoce.Editor.SelectionCommands;
using Thawmadoce.Frame;

namespace Thawmadoce.Extensibility
{
    /// <summary>
    /// Base class for a selection command
    /// </summary>
    public abstract class SelectionCommand : ISelectionCommandWireup
    {
        protected readonly string SelectionText;
        private readonly RelayCommand _command;
        private Action<string> _newSelection;

        protected SelectionCommand(string selectionText)
        {
            SelectionText = selectionText;
            _command = new RelayCommand(InternalExecute);
        }

        public ICommand Command
        {
            get { return _command; }
        }

        /// <summary>
        /// Text to show next to the icon
        /// </summary>
        public string CommandText { get; set; }

        /// <summary>
        /// Path to the icon to show
        /// </summary>
        public string CommandIcon { get; set; }

        /// <summary>
        /// called when the user triggers this command. Return value is the slection after the modification.
        /// </summary>
        protected abstract string Execute();

        private void InternalExecute()
        {
            var newString = Execute();
            if (_newSelection != null)
                _newSelection(newString);
        }

        void ISelectionCommandWireup.AfterModificationCallback(Action<string> newSelection)
        {
            _newSelection = newSelection;
        }
    }
}