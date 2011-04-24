using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CDTag.Common;
using CDTag.FileBrowser.Model;
using CDTag.FileBrowser.ViewModel;

namespace CDTag.FileBrowser.View
{
    /// <summary>
    /// Interaction logic for FileList.xaml
    /// </summary>
    public partial class FileList : UserControl
    {
        private IDirectoryController _directoryController;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileList"/> class.
        /// </summary>
        public FileList()
        {
            InitializeComponent();

            DataContextChanged += FileList_DataContextChanged;
        }

        private void FileList_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _directoryController = (IDirectoryController)DataContext;
            fileView.MouseDoubleClick += fileView_MouseDoubleClick;
        }

        private void fileView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (fileView.SelectedItems.Count == 1)
            {
                FileView item = fileView.SelectedItem as FileView;
                if (item == null)
                    return;

                if (item.IsDirectory)
                {
                    MouseHelper.SetWaitCursor();
                    try
                    {
                        _directoryController.NavigateTo(item.FullName);
                    }
                    finally
                    {
                        MouseHelper.ResetCursor();
                    }
                }
            }
        }
    }
}
