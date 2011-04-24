using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace CDTag.FileBrowser.Behavior
{
    /// <summary>
    /// Removes the Select All (Ctrl+A) command from a DataGrid.
    /// </summary>
    public class RemoveSelectAllBehavior : Behavior<DataGrid>
    {
        /// <summary>Called after the behavior is attached to an AssociatedObject.</summary>
        protected override void OnAttached()
        {
            AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            DependencyObject dep = AssociatedObject;

            while (!(dep is Button))
            {
                dep = VisualTreeHelper.GetChild(dep, 0);
            }

            Button button = (Button)dep;
            
            // Note: This clears ApplicationCommands.SelectAll, be careful reusing this code
            RoutedUICommand routedUICommand = (RoutedUICommand)button.Command;
            routedUICommand.InputGestures.Clear();
            button.Command = null;

            base.OnAttached();
        }
    }
}
