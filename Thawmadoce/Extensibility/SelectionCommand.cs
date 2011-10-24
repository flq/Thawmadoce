using System;
using Thawmadoce.Editor.SelectionCommands;

namespace Thawmadoce.Extensibility
{
    /// <summary>
    /// Base class for a selection command
    /// </summary>
    public abstract class SelectionCommand : VisibleCommand, ISelectionCommandWireup
    {
        protected readonly string SelectionText;
        private Action<string> _newSelection;

        protected SelectionCommand(string selectionText)
        {
            SelectionText = selectionText;
        }

        protected override void InternalExecute()
        {
            var newString = Execute();
            if (_newSelection != null)
                _newSelection(newString);
        }

        void ISelectionCommandWireup.AfterModificationCallback(Action<string> newSelection)
        {
            _newSelection = newSelection;
        }

        /// <summary>
        /// called when the user triggers this command. Return value is the slection after the modification.
        /// </summary>
        protected abstract string Execute();
    }
}