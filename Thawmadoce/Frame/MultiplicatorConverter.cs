using System;
using System.Globalization;
using System.Windows.Data;

namespace Thawmadoce.Frame.Converters
{
    public class MultiplicatorConverter : IValueConverter
    {
        private int _factorInt;
        private double _factorDouble;

        public int Factor
        {
            get { return _factorInt; }
            set
            {
                _factorInt = value;
                _factorDouble = System.Convert.ToDouble(value);
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
                return ((int)value) * _factorInt;
            if (value is double)
                return ((double)value) * _factorDouble;

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Multplicator only supports read-only bindings");
        }
    }
}