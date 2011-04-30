using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CDTag.Common;
using CDTag.FileBrowser.ViewModel;

namespace CDTag.FileBrowser.View
{
    /// <summary>
    /// Interaction logic for FileExplorer.xaml
    /// </summary>
    public partial class FileExplorer : UserControl
    {
        private readonly IDirectoryController _directoryController;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileExplorer"/> class.
        /// </summary>
        public FileExplorer()
        {
            InitializeComponent();

            _directoryController = Unity.Resolve<IDirectoryController>();
            DataContext = _directoryController;

            PreviewMouseDown += FileExplorer_PreviewMouseDown;

            _directoryController.EnhancedPropertyChanged += _directoryController_EnhancedPropertyChanged;
            //DirectoryComboBox.IsTextSearchEnabled = false;
        }

        private void _directoryController_EnhancedPropertyChanged(object sender, EnhancedPropertyChangedEventArgs<IDirectoryController> e)
        {
            if (DirectoryComboBox.IsSelectionBoxHighlighted)
            {
                if (e.IsProperty(p => p.SubDirectories))
                {
                    if (_directoryController.SubDirectories == null || _directoryController.SubDirectories.Count == 0)
                    {
                        DirectoryComboBox.IsDropDownOpen = false;
                    }
                    else
                    {
                        DirectoryComboBox.IsDropDownOpen = true;
                    }
                }
            }
        }

        /// <summary>Gets or sets the toolbar.</summary>
        /// <value>The toolbar.</value>
        public object Toolbar
        {
            get { return ToolbarContentControl.Content; }
            set { ToolbarContentControl.Content = value; }
        }

        private void FileExplorer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.XButton1 == MouseButtonState.Pressed)
            {
                if (_directoryController.GoBackCommand.CanExecute(null))
                {
                    _directoryController.GoBackCommand.Execute(null);
                    e.Handled = true;
                }
            }
            else if (e.XButton2 == MouseButtonState.Pressed)
            {
                if (_directoryController.GoForwardCommand.CanExecute(null))
                {
                    _directoryController.GoForwardCommand.Execute(null);
                    e.Handled = true;
                }
            }
        }

        /// <summary>Gets the directory controller.</summary>
        public IDirectoryController DirectoryController
        {
            get { return _directoryController; }
        }

        /// <summary>Gets the width of the grid splitter.</summary>
        /// <returns>The width of the grid splitter.</returns>
        public GridLength GridSplitterPosition
        {
            get { return LayoutRoot.ColumnDefinitions[0].Width; }
            set { LayoutRoot.ColumnDefinitions[0].Width = value; }
        }

        /// <summary>Gets the file view columns.</summary>
        /// <returns>The file view columns.</returns>
        public ObservableCollection<DataGridColumn> GetFileViewColumns()
        {
            return FileList.GetFileViewColumns();
        }
    }
}
