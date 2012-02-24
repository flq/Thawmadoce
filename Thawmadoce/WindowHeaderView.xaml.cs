using System.Windows;
using System.Windows.Controls;
using DynamicXaml.Extensions;
using DynamicXaml.MarkupSystem;

namespace Thawmadoce
{
    /// <summary>
    /// Interaction logic for WindowHeaderView.xaml
    /// </summary>
    public partial class WindowHeaderView : UserControl
    {
        public WindowHeaderView()
        {
            InitializeComponent();
        }

        private void HandleCloseClick(object sender, RoutedEventArgs e)
        {
            var b = sender as DependencyObject;
            b.GetVisualParent<Window>().Do(wdw => wdw.Close());
        }

        private void OnHeaderMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var b = sender as DependencyObject;
            b.GetVisualParent<Window>().Do(wdw => wdw.DragMove());
        }

        private void HandleMaximizeClick(object sender, RoutedEventArgs e)
        {
            var b = sender as DependencyObject;
            b.GetVisualParent<Window>().Do(w => w.WindowState = w.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal);
        }

        private void HandleMinimizeClick(object sender, RoutedEventArgs e)
        {
            var b = sender as DependencyObject;
            b.GetVisualParent<Window>().Do(wdw => wdw.WindowState = WindowState.Minimized);
        }
    }
}
