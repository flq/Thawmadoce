using System.Windows;
using System.Windows.Media;

namespace Thawmadoce.Frame.Extensions
{
    public static class WpfExtensions
    {
        public static T GetVisualParent<T>(this DependencyObject obj) where T : DependencyObject
        {
            while (obj != null)
            {
                if (obj is T)
                    return (T)obj;
                obj = VisualTreeHelper.GetParent(obj);
            }
            return null;
        }
    }
}