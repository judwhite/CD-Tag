﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CDTag.View.Converters.General
{
    public class IsEqualToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string.Format("{0}", value) == (string)parameter) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
