﻿using System;
using System.Windows.Data;

namespace CDTag.Converters
{
    public class NullableIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            int intValue;
            if (int.TryParse(value.ToString(), out intValue))
                return intValue;
            else
                return null;
        }
    }
}
