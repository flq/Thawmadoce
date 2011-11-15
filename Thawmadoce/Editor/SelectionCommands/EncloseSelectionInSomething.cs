using System.Text;
using Thawmadoce.Extensibility;

namespace Thawmadoce.Editor.SelectionCommands
{
    public class EncloseSelectionInSomething : SelectionCommand
    {
        private readonly string _prefixString;
        private readonly string _postFixString;

        public EncloseSelectionInSomething(TextContext textContext, string enclosingString) : this(textContext, enclosingString, enclosingString)
        {
            
        }

        public EncloseSelectionInSomething(TextContext textContext, string prefixString, string postFixString) : base(textContext)
        {
            _prefixString = prefixString;
            _postFixString = postFixString;
        }

        protected override TextContext Execute()
        {
            var sb = new StringBuilder();

            sb.Append(_prefixString);
            sb.Append(TextContext.CurrentSelection);
            sb.Append(_postFixString);
            TextContext.ReplaceSelection(sb.ToString());
            return  TextContext;
        }
    }
}