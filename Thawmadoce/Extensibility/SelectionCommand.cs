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


        private string _commandText;

        /// <summary>
        /// Text to show next to the icon
        /// </summary>
        public string CommandText
        {
            get { return string.IsNullOrEmpty(CommandIcon) ? _commandText : string.Empty; }
            set { _commandText = value; }
        }

        /// <summary>
        /// Path to the icon to show
        /// </summary>
        public string CommandIcon { get; set; }

        /// <summary>
        /// Set a key combination if you want the command to be accessible through a key combination
        /// </summary>
        public KeyCombo KeyCombination { get; set; }

        public string TooltipText
        {
            get
            {
                var ret = _commandText;
                if (KeyCombination != KeyCombo.Default)
                    ret += " (" + KeyCombination + ")";
                return ret;
            }
        }

        public KeyBinding KeyBinding
        {
            get
            {
                if (KeyCombination == KeyCombo.Default)
                    throw new InvalidOperationException("Will not generate a key binding for an empty key combination");
                return new KeyBinding(Command, KeyCombination.Key, KeyCombination.Modifier);
            }
        }

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