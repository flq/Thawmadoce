using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Thawmadoce.Frame.Extensions;

namespace Thawmadoce.Frame
{
    public static class FocusBehavior
    {
        #region Dependency Properties
        /// <summary>
        /// <c>IsFocused</c> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsFocusedProperty =
        DependencyProperty.RegisterAttached("IsFocused", typeof(bool?),
                typeof(FocusBehavior), new FrameworkPropertyMetadata(IsFocusedChanged));
        /// <summary>
        /// Gets the <c>IsFocused</c> property value.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>Value of the <c>IsFocused</c> property or <c>null</c> if not set.</returns>
        public static bool? GetIsFocused(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return (bool?)element.GetValue(IsFocusedProperty);
        }
        /// <summary>
        /// Sets the <c>IsFocused</c> property value.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="value">The value.</param>
        public static void SetIsFocused(DependencyObject element, bool? value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            element.SetValue(IsFocusedProperty, value);
        }
        #endregion Dependency Properties

        #region Event Handlers
        /// <summary>
        /// Determines whether the value of the dependency property <c>IsFocused</c> has change.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void IsFocusedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Ensure it is a FrameworkElement instance.
            var fe = d as FrameworkElement;
            if (fe != null && e.OldValue == null && e.NewValue != null && (bool)e.NewValue)
            {
                // Attach to the Loaded event to set the focus there. If we do it here it will
                // be overridden by the view rendering the framework element.
                fe.Loaded += FrameworkElementLoaded;
            }
        }
        /// <summary>
        /// Sets the focus when the framework element is loaded and ready to receive input.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private static void FrameworkElementLoaded(object sender, RoutedEventArgs e)
        {
            // Ensure it is a FrameworkElement instance.
            var fe = sender as FrameworkElement;
            if (fe == null) return;

            // Remove the event handler registration.
            fe.Loaded -= FrameworkElementLoaded;

            Debug.WriteLine("Calling focus from attached property on " + fe);
            if (fe is IHandleFocus)
            {
                // Thing implements I handle focus
                ((IHandleFocus)fe).HandleFocus();
                return;
            }
            
            fe.Focus();
            Keyboard.Focus(fe);
            fe.As<TextBoxBase>(tb => tb.SelectAll());
        }
        #endregion Event Handlers
    }

    public interface IHandleFocus
    {
        void HandleFocus();
    }
}
