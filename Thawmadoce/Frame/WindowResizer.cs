using System;
using System.Windows;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.Windows.Threading;

namespace Thawmadoce.Frame
{
    /// <summary>
    /// Inspired by http://www.codeproject.com/KB/WPF/WPF_Window_Resizing.aspx
    /// but heavily modified.
    /// </summary>
    public class WindowResizer
    {
        private readonly Window _target;
        private bool _resizing;
        private PointApi _resizePoint;
        private Size _resizeSize;
        private readonly DispatcherTimer _timer;

        public WindowResizer(Window target, UIElement gripElement)
        {
            _target = target;
            
            _timer = new DispatcherTimer(DispatcherPriority.Send, _target.Dispatcher)
                         {Interval = TimeSpan.FromMilliseconds(10)};
            _timer.Tick += HandleMouseCheck;
            gripElement.MouseLeftButtonDown += HandleElementMouseLeftButtonDown;
        }

        private void HandleElementMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            GetCursorPos(out _resizePoint);
            _resizeSize = new Size(_target.Width, _target.Height);
            _resizing = true;
            _timer.Start();
        }

        private void HandleMouseCheck(object sender, EventArgs e)
        {
            if (_resizing)
            {
                UpdateSize();
                UpdateMouseDown();
            }
            else
                _timer.Stop();
        }

        private void UpdateSize()
        {
            PointApi p;
            GetCursorPos(out p);
            _target.Height = _resizeSize.Height - (_resizePoint.Y - p.Y);
            _target.Width = _resizeSize.Width - (_resizePoint.X - p.X);
        }

        private void UpdateMouseDown()
        {
            if (Mouse.LeftButton == MouseButtonState.Released)
                _resizing = false;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out PointApi lpPoint);

        private struct PointApi
        {
            public int X;
            public int Y;
        }
    }
}
