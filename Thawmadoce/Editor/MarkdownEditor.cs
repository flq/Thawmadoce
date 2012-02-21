﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Thawmadoce.MainApp;

namespace Thawmadoce.Editor
{
    public class MarkdownEditor : TextBox
    {
        private int _lastCaretIndex;
        private bool _runningModification;

        public MarkdownEditor()
        {
            Loaded += HandleLoaded;
            IsVisibleChanged += HandleVisibleChanged;
            SelectionChanged += HandleSelectionChanged;
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

        public static readonly DependencyProperty FocusCommandsProperty =
          DependencyProperty.Register("FocusCommands", typeof(IObservable<IRefocusEditor>), typeof(MarkdownEditor), new PropertyMetadata(HandleFocusCommandsChanged));

        public IObservable<IRefocusEditor> FocusCommands
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
          DependencyProperty.Register("TextAppendCommands", typeof(IObservable<AppendTextUiMsg>), typeof(MarkdownEditor), new PropertyMetadata(HandleTextAppendCommandsChanged));

        public IObservable<AppendTextUiMsg> TextAppendCommands
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
    }
}