using System;
using System.Windows.Data;

namespace CDTag.FileBrowser.Converters
{
    /// <summary>
    /// StatusBarConverter
    /// </summary>
    public class StatusBarConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts source values to a value for the binding target. The data binding engine calls this method when it propagates the values from source bindings to the binding target.
        /// </summary>
        /// <param name="values">The array of values that the source bindings in the <see cref="T:System.Windows.Data.MultiBinding"/> produces. The value <see cref="F:System.Windows.DependencyProperty.UnsetValue"/> indicates that the source binding has no value to provide for conversion.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value.If the method returns null, the valid null value is used.A return value of <see cref="T:System.Windows.DependencyProperty"/>.<see cref="F:System.Windows.DependencyProperty.UnsetValue"/> indicates that the converter did not produce a value, and that the binding will use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue"/> if it is available, or else will use the default value.A return value of <see cref="T:System.Windows.Data.Binding"/>.<see cref="F:System.Windows.Data.Binding.DoNothing"/> indicates that the binding does not transfer the value or use the <see cref="P:System.Windows.Data.BindingBase.FallbackValue"/> or the default value.
        /// </returns>
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int itemCount = (int)values[0];
            long bytes = (long)values[1];

            // TODO: Localization
            const string OneItem = "1 item";
            const string ManyItems = "{0:#,0} items";
            const string StatusBarKB = " ({0:#,0} KB)";
            const string StatusBarMB = " ({0:#,0.0} MB)";
            const string StatusBarGB = " ({0:#,0.0} GB)";

            decimal kb = bytes / 1024.0m;
            decimal mb = kb / 1024.0m;
            decimal gb = mb / 1024.0m;

            string text;
            if (itemCount == 1)
                text = OneItem;
            else
                text = string.Format(ManyItems, itemCount);

            if (bytes <= 0) { /* do nothing */ }
            else if (bytes < 1024 * 1024)
                text += string.Format(StatusBarKB, kb);
            else if (bytes < 1024 * 1024 * 1024)
                text += string.Format(StatusBarMB, mb);
            else
                text += string.Format(StatusBarGB, gb);

            return text;
        }

        /// <summary>Converts a binding target value to the source binding values.</summary>
        /// <param name="value">The value that the binding target produces.</param>
        /// <param name="targetTypes">The array of types to convert to. The array length indicates the number and types of values that are suggested for the method to return.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// An array of values that have been converted from the target value back to the source values.
        /// </returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
