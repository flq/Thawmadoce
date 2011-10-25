using System.Collections.Generic;

namespace Thawmadoce.Extensibility
{
    /// <summary>
    /// Implement to provide Buttons, Keyboard Shortcuts and the corresponding commands to the App-Items menu.
    /// </summary>
    public interface IAppItemsPlugin
    {
        IEnumerable<IVisibleCommand> GetCommands();
    }
}