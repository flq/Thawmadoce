using System.Windows.Input;

namespace Thawmadoce.Extensibility
{
    public interface IVisibleCommand
    {
        ICommand Command { get; }

        /// <summary>
        /// Text to show if an icon is missing. If an icon is available, it will be used as tooltip
        /// </summary>
        string CommandText { get; set; }

        /// <summary>
        /// Path to the icon to show
        /// </summary>
        string CommandIcon { get; set; }

        /// <summary>
        /// Set a key combination if you want the command to be accessible through a key combination
        /// </summary>
        KeyCombo KeyCombination { get; set; }

        KeyBinding KeyBinding { get; }

        bool HasKeyBinding { get; }
    }
}