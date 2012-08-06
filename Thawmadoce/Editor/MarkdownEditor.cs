using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;
using Thawmadoce.MainApp;

namespace Thawmadoce.Editor
{
    public class MarkdownEditor : TextBox
    {
        private int _lastCaretIndex;
        private bool _runningSelectionLogic;

        public MarkdownEditor()
        {
            Loaded += HandleLoaded;
            IsVisibleChanged += HandleVisibleChanged;
            SelectionChanged += HandleSelectionChanged;
            //this.DeclareChangeBlock()
            //this.GetRectFromCharacterIndex()
            //this.LockCurrentUndoUnit();
        }

        public static readonly DependencyProperty CurrentSelectionProperty =
            DependencyProperty.Register("CurrentSelection", typeof (string), typeof (MarkdownEditor), new PropertyMetadata(HandleCurrentSelectionChanged));

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
            me.HandleNewSelection((string)e.NewValue);
        }

        private void HandleNewSelection(string newValue)
        {
            if (Execute.InDesignMode) return;
            _runningSelectionLogic = true;
            try
            {
                var potentialExistingText = Text.IndexOf(newValue, StringComparison.InvariantCulture);
                if (potentialExistingText > -1)
                {
                    Select(potentialExistingText, newValue.Length);
                    _lastCaretIndex = potentialExistingText;
                }
                else
                {

                    var idxStart = SelectionStart;
                    var newValueLength = newValue.Length;
                    LockCurrentUndoUnit();
                    using (DeclareChangeBlock())
                    {
                        SelectedText = newValue;
                    }
                    _runningSelectionLogic = false;
                    _lastCaretIndex = idxStart + newValueLength;
                    Select(_lastCaretIndex, 0);
                    CurrentSelection = string.Empty;
                    FocusMeth();
                }
            }
            finally
            {
                
                _runningSelectionLogic = false;
            }
        }

        public static readonly DependencyProperty FocusCommandsProperty =
          DependencyProperty.Register("RefocusStream", typeof(IObservable<IRefocusEditor>), typeof(MarkdownEditor), new PropertyMetadata(HandleFocusCommandsChanged));

        public IObservable<IRefocusEditor> RefocusStream
        {
            get { return (IObservable<IRefocusEditor>)GetValue(FocusCommandsProperty); }
            set { SetValue(FocusCommandsProperty, value); }
        }

        private static void HandleFocusCommandsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var me = (MarkdownEditor)d;
                ((IObservable<IRefocusEditor>)e.NewValue).Subscribe(_ => me.FocusMeth());
            }
        }

        public static readonly DependencyProperty TextAppendCommandsProperty =
          DependencyProperty.Register("TextAppendStream", typeof(IObservable<AppendTextUiMsg>), typeof(MarkdownEditor), new PropertyMetadata(HandleTextAppendCommandsChanged));

        public IObservable<AppendTextUiMsg> TextAppendStream
        {
            get { return (IObservable<AppendTextUiMsg>)GetValue(FocusCommandsProperty); }
            set { SetValue(FocusCommandsProperty, value); }
        }

        private static void HandleTextAppendCommandsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var me = (MarkdownEditor)d;
                ((IObservable<AppendTextUiMsg>)e.NewValue).Subscribe(msg => me.AppendText(msg.Text));
            }
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
            }
            else
            {
                if (CaretIndex > 0)
                    _lastCaretIndex = CaretIndex;
            }
        }

        private void HandleSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (_runningSelectionLogic)
                return;
            CurrentSelection = SelectedText;
        }

        private void FocusMeth()
        {
            Focus();
            Keyboard.Focus(this);
            if (!string.IsNullOrEmpty(SelectedText))
            {
                int idx = GetLineIndexFromCharacterIndex(SelectionStart);
                ScrollToLine(idx);
            }
            else
            {
                if (_lastCaretIndex > 0)
                    CaretIndex = _lastCaretIndex;

                int lineIndex = GetLineIndexFromCharacterIndex(CaretIndex);
                ScrollToLine(lineIndex);
            }
        }
    }
}
