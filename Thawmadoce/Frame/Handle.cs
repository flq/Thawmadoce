using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Thawmadoce.Frame.Extensions;

namespace Thawmadoce.Frame
{
    public class Handle : Control
    {
        private WindowResizer wdwR;

        public Handle()
        {
            Loaded += HandleLoaded;
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            var wdw = this.GetVisualParent<Window>();
            if (wdw == null)
                return; //WTF!
            wdwR = new WindowResizer(wdw, this);
        }


    }
}