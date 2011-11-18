using System.Windows;
using System.Windows.Controls;
using Thawmadoce.Frame.Extensions;

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
            var wdw = b.GetVisualParent<Window>();

            if (wdw != null)
                wdw.Close();
        }

        private void OnHeaderMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var b = sender as DependencyObject;
            var w = b.GetVisualParent<Window>();
            if (w != null)
                w.DragMove();
        }
    }
}
