using System;
using System.Collections.Generic;
using System.Windows.Input;
using Thawmadoce.Extensibility;

namespace Thawmadoce.MainApp
{
    public class MainAppShortcuts : IKeyboardOnlyActions
    {
        public IEnumerable<KeyComboToMessage> GetKeyboardActions()
        {
            yield return new KeyComboToMessage
                             {
                                 Keys = new KeyCombo(Key.Return, ModifierKeys.Control),
                                 MessageFactory = () => new ToggleAppMenuUiMsg()
                             };
        }
    }
}