using System;
using Thawmadoce.Editor.SelectionCommands;

namespace Thawmadoce.Extensibility
{
    /// <summary>
    /// Base class for a selection command
    /// </summary>
    public abstract class SelectionCommand : VisibleCommand, ISelectionCommandWireup
    {
        protected readonly TextContext TextContext;
        private Action<TextContext> _newSelection;

        protected SelectionCommand(TextContext textContext)
        {
            TextContext = textContext;
        }

        protected override void InternalExecute()
        {
            var newString = Execute();
            if (_newSelection != null)
                _newSelection(newString);
        }

        void ISelectionCommandWireup.AfterModificationCallback(Action<TextContext> newSelection)
        {
            _newSelection = newSelection;
        }

        /// <summary>
        /// called when the user triggers this command. Return value is the slection after the modification.
        /// </summary>
        protected abstract TextContext Execute();
    }
}