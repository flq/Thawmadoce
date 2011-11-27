using System;
using System.Windows;

namespace Thawmadoce.Extensibility
{
    /// <summary>
    /// Express in a dialog view model that you want to use the IDialogRemoteControl
    /// to control when to close the window and what title to set.
    /// </summary>
    public interface INeedRemoteControl
    {
        void Accept(IDialogRemoteControl remoteControl);
    }

    /// <summary>
    /// Helps you to control the running dialog.
    /// </summary>
    public interface IDialogRemoteControl
    {
        void SetDialogTitle(string title);

        /// <summary>
        /// When called closes the dialog, by default with a positive result
        /// </summary>
        void CloseDialog();
    }

    class DialogRemoteControl : IDialogRemoteControl
    {
        private readonly Window _window;

        public DialogRemoteControl(Window window)
        {
            _window = window;
        }

        public void SetDialogTitle(string title)
        {
            _window.Title = title;
        }

        public void CloseDialog()
        {
            _window.DialogResult = true;
            _window.Close();
        }
    }
}