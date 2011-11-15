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
            var nextRefId = TextContext.NextReferenceId;
            TextContext.ReplaceSelection("[{0}][{1}]", TextContext.CurrentSelection, nextRefId);
            TextContext.AppendReference("http://whatafunthing");
            return TextContext;
        }
    }
}