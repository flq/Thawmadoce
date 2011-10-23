using System.Collections.Generic;

namespace Thawmadoce.Extensibility
{
    /// <summary>
    /// Gets called when the user has selected some text in the markdown editor. 
    /// You get to look at the text after which you can return a list of
    /// Selection commands.
    /// </summary>
    public interface ISelectionPlugin
    {
        IEnumerable<SelectionCommand> GetCommands(string selectionText);
    }
}