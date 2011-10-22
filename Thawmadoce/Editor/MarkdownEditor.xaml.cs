using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Thawmadoce.Editor
{
    public partial class MarkdownEditor : TextBox
    {
        private int _lastCaretIndex;

        public MarkdownEditor()
        {
            InitializeComponent();
            Loaded += HandleLoaded;
            IsVisibleChanged += HandleVisibleChanged;
        }

        private void HandleVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                FocusMeth();
            }
            else
            {
                if (CaretIndex > 0)
                    _lastCaretIndex = CaretIndex;
            }
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            FocusMeth();
        }

        private void FocusMeth()
        {
            if (_lastCaretIndex > 0)
                CaretIndex = _lastCaretIndex;
            Focus();
            Keyboard.Focus(this);
        }
    }
}
