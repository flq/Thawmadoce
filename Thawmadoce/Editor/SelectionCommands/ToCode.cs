using System;

namespace Thawmadoce.Editor.SelectionCommands
{
    public class ToCode : PrependLines
    {
        public ToCode(string selectionText) : base(selectionText, "    ")
        {
        }

        protected override string Execute()
        {
            if (SelectionText.Contains(Environment.NewLine))
                return base.Execute();
            return "`" + SelectionText + "`";
        }
    }
}