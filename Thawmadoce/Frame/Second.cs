using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Thawmadoce.Frame
{
    public class Second
    {
        public static readonly DependencyProperty SourceProperty =
        DependencyProperty.RegisterAttached("Source", typeof(ImageSource), typeof(Second), new PropertyMetadata(new PropertyChangedCallback(HandleValueChanged)));

        private static readonly Dictionary<Image,ImageSource> _sourceCache = new Dictionary<Image, ImageSource>();

        private static void HandleValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var img = d as Image;
            if (img == null) return;

            img.MouseEnter += HandleMouseEnter;
            img.MouseLeave += HandleMouseLeave;
        }

        private static void HandleMouseLeave(object sender, MouseEventArgs e)
        {
            var img = sender as Image;
            if (img == null) return;
            img.Source = _sourceCache[img];
        }

        private static void HandleMouseEnter(object sender, MouseEventArgs e)
        {
            var img = sender as Image;
            if (img == null) return;
            _sourceCache[img] = img.Source;
            img.Source = GetSource(img);
        }

        public static ImageSource GetSource(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return (ImageSource)element.GetValue(SourceProperty);
        }

        public static void SetSource(DependencyObject element, ImageSource value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            element.SetValue(SourceProperty, value);
        }
    }
}