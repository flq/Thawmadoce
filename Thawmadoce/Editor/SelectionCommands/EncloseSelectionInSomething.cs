using System.Text;
using Thawmadoce.Extensibility;

namespace Thawmadoce.Editor.SelectionCommands
{
    public class EncloseSelectionInSomething : SelectionCommand
    {
        private readonly string _enclosingString;

        public EncloseSelectionInSomething(string selectionText, string enclosingString) : base(selectionText)
        {
            _enclosingString = enclosingString;
        }

        protected override string Execute()
        {
            var sb = new StringBuilder();
            sb.Append(_enclosingString);
            sb.Append(SelectionText);
            sb.Append(_enclosingString);
            return sb.ToString();
        }
    }
}