using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using CDTag.FileBrowser.ViewModel;

namespace CDTag.FileBrowser.Converters
{
    /// <summary>
    /// DirectoryMenuItemConverter
    /// </summary>
    public class DirectoryMenuItemConverter : IMultiValueConverter
    {
        private IDirectoryController _directoryController;

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
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<MenuItem> menuItems = new ObservableCollection<MenuItem>();

            if (values.Length != 2)
                return menuItems;

            _directoryController = (IDirectoryController)values[0];
            if (_directoryController == null)
                return menuItems;

            var dirs = _directoryController.SubDirectories;

            if (dirs == null || dirs.Count == 0)
                return menuItems;
            
            foreach (var item in dirs)
            {
                TextBlock header = new TextBlock
                {
                    Text = item,
                };
                MenuItem subDirMenuItem = new MenuItem { Header = header, Tag = item };
                subDirMenuItem.PreviewKeyDown += KeyEventHandler;
                subDirMenuItem.Click += subDirMenuItem_Click;

                menuItems.Add(subDirMenuItem);
            }

            return menuItems;
        }

        private void subDirMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Navigate, close popup, lose focus

            MenuItem menuItem = (MenuItem)sender;
            string directory = (string)menuItem.Tag;

            _directoryController.CurrentDirectory = directory;
            _directoryController.HideAddressTextBox();
        }

        private void KeyEventHandler(object sender, KeyEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            string directory = (string)menuItem.Tag;

            if (e.Key == Key.Escape)
            {
                // Keep dir, close popup, focus nav bar
                _directoryController.ClosePopup();
                _directoryController.TypingDirectory = directory;
                _directoryController.FocusAddressTextBox();
                e.Handled = true;
            }
            else if (e.Key == Key.Oem5)
            {
                // Backslash

                // Navigate, keep popup open, focus address bar
                _directoryController.TypingDirectory = directory + @"\";
                _directoryController.FocusAddressTextBox();
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                // Navigate, close popup, lose focus
                _directoryController.HideAddressTextBox();
                _directoryController.CurrentDirectory = directory;
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                // do nothing, let popup handle it
            }
            else if (e.Key == Key.Up)
            {
                // TODO: if first item, focus address bar
            }
        }

        /// <summary>Converts a binding target value to the source binding values.</summary>
        /// <param name="value">The value that the binding target produces.</param>
        /// <param name="targetTypes">The array of types to convert to. The array length indicates the number and types of values that are suggested for the method to return.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// An array of values that have been converted from the target value back to the source values.
        /// </returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
