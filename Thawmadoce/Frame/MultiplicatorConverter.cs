using System;
using System.Globalization;
using System.Windows.Data;

namespace Thawmadoce.Frame
{
    public class MultiplicatorConverter : IValueConverter
    {
        public int Factor { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var multiplicator = Factor != 0 ? Factor : 1;
            if (!(value is int))
                return 0;

            var conversion = ((int)value) * multiplicator;
            return conversion;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("Multplicator only supports read-only bindings");
        }
    }
}