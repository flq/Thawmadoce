using System;
using System.Windows.Input;
using Thawmadoce.Frame;

namespace Thawmadoce.Extensibility
{
    /// <summary>
    /// Base class for a visible command
    /// </summary>
    public abstract class VisibleCommand : IVisibleCommand
    {
        private readonly RelayCommand _command;
        private string _commandText;

        public ICommand Command
        {
            get { return _command; }
        }

        protected VisibleCommand()
        {
            _command = new RelayCommand(InternalCanExecute, InternalExecute);
        }

        /// <summary>
        /// Text to show next to the icon
        /// </summary>
        public string CommandText
        {
            get { return string.IsNullOrEmpty(CommandIcon) ? _commandText : string.Empty; }
            set { _commandText = value; }
        }

        /// <summary>
        /// Path to the icon to show
        /// </summary>
        public string CommandIcon { get; set; }

        /// <summary>
        /// Set a key combination if you want the command to be accessible through a key combination
        /// </summary>
        public KeyCombo KeyCombination { get; set; }

        public string TooltipText
        {
            get
            {
                var ret = _commandText;
                if (KeyCombination != KeyCombo.Default)
                    ret += " (" + KeyCombination + ")";
                return ret;
            }
        }

        public KeyBinding KeyBinding
        {
            get
            {
                if (KeyCombination == KeyCombo.Default)
                    throw new InvalidOperationException("Will not generate a key binding for an empty key combination");
                return new KeyBinding(Command, KeyCombination.Key, KeyCombination.Modifier);
            }
        }

        protected virtual bool InternalCanExecute()
        {
            return true;
        }

        protected virtual void InternalExecute() { }
    }
}