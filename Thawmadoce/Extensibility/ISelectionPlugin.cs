using System;
using System.Collections.Generic;

namespace Thawmadoce.Extensibility
{
    /// <summary>
    /// Gets called when the user has selected some text in the markdown editor. 
    /// You get to look at the text after which you can return a list of
    /// Selection commands.
    /// </summary>
    public interface ISelectionPlugin
    {
        IEnumerable<IVisibleCommand> GetCommands(TextContext selectionText);
    }

    /// <summary>
    /// The current situation with regard to the text typed by the user
    /// </summary>
    public class TextContext
    {
        private string _currentSelection;
        private string _completeText;

        public TextContext(string currentSelection, string completeText)
        {
            _currentSelection = currentSelection;
            _completeText = completeText;
        }

        public string CurrentSelection
        {
            get { return _currentSelection; }
            set
            {
                _currentSelection = value;
                CurrentSelectionChanged = true;
            }
        }

        public string CompleteText
        {
            get { return _completeText; }
            set
            {
                _completeText = value;
                CompleteTextChanged = true;
            }
        }

        public bool CurrentSelectionChanged { get; private set; }
        public bool CompleteTextChanged { get; private set; }

        public void ReplaceSelection(string newSelection)
        {
            CurrentSelection = newSelection;
        }

        public void ReplaceInsideSelection(string old, string @new)
        {
            CurrentSelection = CurrentSelection.Replace(old, @new);
        }
    }
}