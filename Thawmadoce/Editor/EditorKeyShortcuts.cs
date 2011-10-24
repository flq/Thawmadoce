using System.Collections.Generic;
using System.Windows.Input;
using Thawmadoce.Extensibility;

namespace Thawmadoce.Editor
{
    public class EditorKeyShortcuts : IKeyboardOnlyActions
    {
        public IEnumerable<KeyComboToMessage> GetKeyboardActions()
        {
            yield return new KeyComboToMessage
                             {
                                 Keys = new KeyCombo(Key.Tab, ModifierKeys.Control), 
                                 MessageFactory = () => new TogglePreviewUiMsg()
                             };
        }
    }
}