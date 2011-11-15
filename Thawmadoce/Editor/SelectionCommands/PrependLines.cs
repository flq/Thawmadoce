using System;
using Thawmadoce.Extensibility;

namespace Thawmadoce.Editor.SelectionCommands
{
    public class PrependLines : SelectionCommand
    {
        private readonly string _prefix;

        public PrependLines(TextContext textContext, string prefix) : base(textContext)
        {
            _prefix = prefix;
        }

        protected override TextContext Execute()
        {
            TextContext.ReplaceSelection(_prefix + TextContext.CurrentSelection.Replace(Environment.NewLine, Environment.NewLine + _prefix));
            return TextContext;
        }
    }
}