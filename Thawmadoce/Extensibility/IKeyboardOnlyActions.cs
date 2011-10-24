using System;
using System.Collections.Generic;

namespace Thawmadoce.Extensibility
{
    /// <summary>
    /// Allows you to register the publishing of messages triggered by a keyboard command.
    /// You can use this if some command is _only_ triggered by a KeyCombo. You can then capture 
    /// the message on e.g. an <see cref="ISaga" /> implementation or a ViewModel.
    /// </summary>
    public interface IKeyboardOnlyActions
    {
        IEnumerable<KeyComboToMessage> GetKeyboardActions();
    }

    /// <summary>
    /// A key combination that releases a message into the system
    /// </summary>
    public class KeyComboToMessage
    {
        /// <summary>
        /// The triggering key combination
        /// </summary>
        public KeyCombo Keys { get; set; }

        /// <summary>
        /// The function that provides a message to be published
        /// </summary>
        public Func<object> MessageFactory { get; set;}
    }
}