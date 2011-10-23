using System;
using System.Collections.Generic;
using Thawmadoce.Extensibility;

namespace Thawmadoce.Editor.SelectionCommands
{
    public class StandardMarkdownCommands : ISelectionPlugin
    {
        public IEnumerable<SelectionCommand> GetCommands(string selectionText)
        {
            yield return new EncloseSelectionInSomething(selectionText, "__") { CommandText = "Bold" };
            yield return new EncloseSelectionInSomething(selectionText, "_") { CommandText = "Emphasis" };
        }
    }
}