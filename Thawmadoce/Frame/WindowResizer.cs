using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using DynamicXaml.MarkupSystem;
using DynamicXaml.Extensions;

namespace Thawmadoce.Frame
{
    public class ResizeHandle : Control
    {
        private WindowResizer wdwR;

        public ResizeHandle()
        {
            Loaded += HandleLoaded;
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            wdwR = this.GetVisualParent<Window>().Get(w => new WindowResizer(w, this)).MustHaveValue();
        }
    }

    /// <summary>
    /// Used from http://blog.kirupa.com/?p=256
    /// </summary>
    public class WindowResizer
    {
        private const int WmSyscommand = 0x112;
        private readonly HwndSource hwndSource;

        public enum ResizeDirection
        {
            BottomRight = 8,
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public WindowResizer(Window target, UIElement gripElement)
        {
            hwndSource = PresentationSource.FromVisual(target) as HwndSource;
            gripElement.PreviewMouseDown += HandleMouseDown;
        }

        private void HandleMouseDown(object sender, MouseButtonEventArgs e)
        {
            ResizeWindow(ResizeDirection.BottomRight);
        }

        private void ResizeWindow(ResizeDirection direction)
        {
            SendMessage(hwndSource.Handle, WmSyscommand, (IntPtr)(61440 + direction), IntPtr.Zero);
        }
    }
        
}
