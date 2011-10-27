using System.Windows;
using System.Windows.Controls;

namespace Thawmadoce.Frame
{
    public class ItemsWithContentContainer : ItemsControl
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ContentControl();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            // Even wrap other ContentControls
            return false;
        }
    }
}