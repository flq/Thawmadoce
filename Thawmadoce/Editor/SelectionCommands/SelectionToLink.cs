using System;
using Thawmadoce.Extensibility;

namespace Thawmadoce.Editor.SelectionCommands
{
    public class SelectionToLink : SelectionCommand
    {
        public SelectionToLink(TextContext selectionText) : base(selectionText)
        {
        }

        protected override TextContext Execute()
        {
            return TextContext;
        }
    }
}