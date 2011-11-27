using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Thawmadoce.Frame
{
    public static class QuickGrid
    {
        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.RegisterAttached(
          "Columns",
          typeof(string),
          typeof(QuickGrid),
          new PropertyMetadata(new PropertyChangedCallback(HandleColumnsChanged))
        );

        public static void SetColumns(Grid element, Boolean value)
        {
            element.SetValue(ColumnsProperty, value);
        }

        public static string GetColumns(Grid element)
        {
            return (string)element.GetValue(ColumnsProperty);
        }

        private static void HandleColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var g = (Grid)d;
            var newValue = e.NewValue as string;
            if (string.IsNullOrWhiteSpace(newValue))
                return;

            foreach (var gl in new GridLengthBuilder(newValue).GetLengths())
                g.ColumnDefinitions.Add(new ColumnDefinition { Width = gl});
        }

        public static readonly DependencyProperty RowsProperty = DependencyProperty.RegisterAttached(
          "Rows",
          typeof(string),
          typeof(QuickGrid),
          new PropertyMetadata(new PropertyChangedCallback(HandleRowsChanged))
        );

        public static void SetRows(Grid element, Boolean value)
        {
            element.SetValue(RowsProperty, value);
        }

        public static string GetRows(Grid element)
        {
            return (string)element.GetValue(RowsProperty);
        }

        private static void HandleRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var g = (Grid)d;
            var newValue = e.NewValue as string;
            if (string.IsNullOrWhiteSpace(newValue))
                return;

            foreach (var gl in new GridLengthBuilder(newValue).GetLengths())
                g.RowDefinitions.Add(new RowDefinition { Height = gl });
        }

        public class GridLengthBuilder
        {
            private readonly string _input;

            public GridLengthBuilder(string input)
            {
                _input = input;
            }

            public IEnumerable<GridLength> GetLengths()
            {
                return Enumerable.Empty<GridLength>();
            }
        }
    }
}