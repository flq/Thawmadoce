using System.Text;
using Thawmadoce.Extensibility;

namespace Thawmadoce.Editor.SelectionCommands
{
    public class EncloseSelectionInSomething : SelectionCommand
    {
        private readonly string _prefixString;
        private readonly string _postFixString;

        public EncloseSelectionInSomething(string selectionText, string enclosingString) : this(selectionText, enclosingString, enclosingString)
        {
            
        }

        public EncloseSelectionInSomething(string selectionText, string prefixString, string postFixString) : base(selectionText)
        {
            _prefixString = prefixString;
            _postFixString = postFixString;
        }

        protected override string Execute()
        {
            var sb = new StringBuilder();
            sb.Append(_prefixString);
            sb.Append(SelectionText);
            sb.Append(_postFixString);
            return sb.ToString();
        }
    }
}