using System;
using System.Globalization;
using System.Windows.Data;
using IdSharp.Tagging.ID3v1;

namespace CDTag.Converters
{
    public class ID3v1GenreIndexToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GenreHelper.GenreByIndex[(int)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GenreHelper.GetGenreIndex((string)value);
        }
    }
}
