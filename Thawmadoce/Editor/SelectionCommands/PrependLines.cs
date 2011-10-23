using System;
using Thawmadoce.Extensibility;

namespace Thawmadoce.Editor.SelectionCommands
{
    public class PrependLines : SelectionCommand
    {
        private readonly string _prefix;

        public PrependLines(string selectionText, string prefix) : base(selectionText)
        {
            _prefix = prefix;
        }

        protected override string Execute()
        {
            return _prefix + SelectionText.Replace(Environment.NewLine, Environment.NewLine + _prefix);
        }
    }
}