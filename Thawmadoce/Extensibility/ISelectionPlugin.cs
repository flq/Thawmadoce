using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace Thawmadoce.Extensibility
{
    /// <summary>
    /// The current situation with regard to the text typed by the user
    /// </summary>
    public class TextContext
    {
        /// <summary>
        /// sigh...
        /// Match a line starting with 2 spaces followed by a number of digits in square brackets, followed by :, followed by any chars up to the end of the line
        /// </summary>
        private static readonly Regex _referenceFinder = new Regex(@"^\s\s\[(?'idx'\d+?)\]:\s.*?$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline);
        private readonly List<string> _appendages = new List<string>();
        private int? _cachedTextReference;
        private string _currentSelection;

        public TextContext(string currentSelection, string completeText)
        {
            _currentSelection = currentSelection;
            CompleteText = completeText;
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

        public string CompleteText { get; private set; }

        public bool CurrentSelectionChanged { get; private set; }

        public void AppendReference(string referenceText)
        {
            _appendages.Add(referenceText);
        }

        public bool HasTextToAppend { get { return _appendages.Count > 0; } }

        public string TextToAppend
        {
            get
            {
                if (!HasTextToAppend) return null;
                var nextRef = NextRefFromCurrentText();
                
                var sb = new StringBuilder();

                if (!CompleteText.EndsWith("\n") && !IsLastLineRef())
                    sb.AppendLine();
                sb.AppendLine();

                foreach (var appendage in _appendages)
                {
                    sb.AppendFormat("[{0}]: {1}{2}", nextRef, appendage, Environment.NewLine);
                    nextRef++;
                }
                return sb.ToString();
            }
        }

        public int NextReferenceId
        {
            get
            {
                var currentRef = NextRefFromCurrentText();
                return currentRef + _appendages.Count;
            }
        }

        private int NextRefFromCurrentText()
        {
            if (_cachedTextReference.HasValue)
                return _cachedTextReference.Value;
            var idx = FindLastReference();
            _cachedTextReference = idx + 1;
            return _cachedTextReference.Value;
        }


        public void ReplaceSelection(string newSelection)
        {
            CurrentSelection = newSelection;
        }

        public void ReplaceSelection(string newSelection, params object[] args)
        {
            CurrentSelection = string.Format(newSelection, args);
        }

        private int FindLastReference()
        {
            var mc = _referenceFinder.Matches(CompleteText);
            if (mc.Count == 0)
                return 0;
            return mc.OfType<Match>().Select(m => AsInt(m.Groups["idx"].Value)).Max();
        }

        private bool IsLastLineRef()
        {
            var lastLine = CompleteText.FindLastNonEmptyLine();
            return _referenceFinder.IsMatch(lastLine);
        }

        private static int AsInt(string value)
        {
            int num;
            return (int.TryParse(value, out num)) ? num : -1;
        }
    }

    /// <summary>
    /// Gets called when the user has selected some text in the markdown editor. 
    /// You get to look at the text after which you can return a list of
    /// Selection commands.
    /// </summary>
    public interface ISelectionPlugin
    {
        IEnumerable<IVisibleCommand> GetCommands(TextContext selectionText);
    }
}