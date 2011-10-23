using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Thawmadoce.Editor
{
    public enum ModKeys
    {
        None = 0,
        Ctrl = 1,
        Shift = 2
    }

    public partial class MarkdownEditor : TextBox
    {
        private int _lastCaretIndex;

        private ModKeys _currentModKeys = ModKeys.None;

        public MarkdownEditor()
        {
            InitializeComponent();
            TextWrapping = TextWrapping.Wrap;
            AcceptsReturn = true;
            AcceptsTab = true;
            Loaded += HandleLoaded;
            IsVisibleChanged += HandleVisibleChanged;
            PreviewKeyDown += HandlePreviewKeyDown;
            PreviewKeyUp += HandlePreviewKeyUp;
            SelectionChanged += HandleSelectionChanged;
        }

        public static readonly DependencyProperty CurrentSelectionProperty =
            DependencyProperty.Register("CurrentSelection", typeof (string), typeof (MarkdownEditor), new PropertyMetadata(HandleCurrentSelectionChanged));

        private bool _runningModification;

        public string CurrentSelection
        {
            get { return (string) GetValue(CurrentSelectionProperty); }
            set { SetValue(CurrentSelectionProperty, value); }
        }

        private static void HandleCurrentSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = (MarkdownEditor)d;
            if (Equals(e.NewValue, me.SelectedText))
                return;
            me.IncludeModification((string)e.NewValue);
        }

        private void IncludeModification(string newValue)
        {
            _runningModification = true;
            var idxStart = SelectionStart;
            var idxLength = SelectionLength;
            var newValueLength = newValue.Length;
            var currentText = Text;

            Clear();
            AppendText(currentText.Substring(0,idxStart));
            AppendText(newValue);
            var startOfSecondHalf = idxStart + idxLength;
            AppendText(currentText.Substring(startOfSecondHalf));
            _runningModification = false;
            _lastCaretIndex = idxStart + newValueLength;
            SelectedText = string.Empty;
            CurrentSelection = string.Empty;
            FocusMeth();
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            FocusMeth();
        }

        private void HandleVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                FocusMeth();
                _currentModKeys = ModKeys.None;
            }
            else
            {
                if (CaretIndex > 0)
                    _lastCaretIndex = CaretIndex;
            }
        }

        private void HandleSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (_runningModification)
                return;
            CurrentSelection = SelectedText;
        }

        private void FocusMeth()
        {
            if (_lastCaretIndex > 0)
                CaretIndex = _lastCaretIndex;
            Focus();
            Keyboard.Focus(this);
        }

        private void HandlePreviewKeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.Key);
            if (ControlPressed(e))
                _currentModKeys |= ModKeys.Ctrl;
            if (ShiftPressed(e))
                _currentModKeys |= ModKeys.Shift;
        }

        private void HandlePreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (ControlPressed(e))
                _currentModKeys &= ModKeys.Ctrl;
            if (ShiftPressed(e))
                _currentModKeys &= ModKeys.Shift;
        }

        private static bool ControlPressed(KeyEventArgs e)
        {
            return e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl;
        }

        private static bool ShiftPressed(KeyEventArgs e)
        {
            return e.Key == Key.LeftShift || e.Key == Key.RightShift;
        }
    }
}
