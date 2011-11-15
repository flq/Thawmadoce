using System;
using Thawmadoce.Extensibility;

namespace Thawmadoce.Editor.SelectionCommands
{
    public class ToCode : PrependLines
    {
        public ToCode(TextContext textContext) : base(textContext, "    ")
        {
        }

        protected override TextContext Execute()
        {
            var selection = TextContext.CurrentSelection;
            if (selection.Contains(Environment.NewLine))
                return base.Execute();
            TextContext.ReplaceSelection("`" + selection + "`");
            return TextContext;
        }
    }
}