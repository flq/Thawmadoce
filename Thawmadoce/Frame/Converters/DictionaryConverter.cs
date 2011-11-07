using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Thawmadoce.Frame.Converters
{
    /// <summary>
    /// Example usage:
    /// <DictionaryConverter.Values>
    ///   <Hashtable>
    ///     <String x:Key="{x:Static UI:InteractionKind.Information}">/Lib;component/media/info.png</System:String>
    ///     <String x:Key="{x:Static UI:InteractionKind.Warning}">/Lib;component/media/Warning.png</System:String>
    ///     <String x:Key="{x:Static UI:InteractionKind.Error}">/Lib;component/media/Error.png</System:String>
    ///     <String x:Key="{x:Static UI:InteractionKind.Question}">/Lib;component/media/Question.png</System:String>
    ///   </Hashtable>
    /// </DictionaryConverter.Values>
    /// </summary>
    [ContentProperty("Values")]
    public class DictionaryConverter : IValueConverter
    {

        public System.Collections.Hashtable Values { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Values != null && Values.ContainsKey(value))
            {
                var convert = Values[value];
                return convert;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Only one-way conversion allowed");
        }
    }
}