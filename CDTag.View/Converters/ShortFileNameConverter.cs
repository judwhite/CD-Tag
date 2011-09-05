using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace CDTag.Converters
{
    public class ShortFileNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Path.GetFileName(string.Format("{0}", value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
