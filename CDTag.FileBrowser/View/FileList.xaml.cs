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

        internal void UpdateStatusBar()
        {
            // TODO: Localization
            const string OneItem = "1 item";
            const string ManyItems = "{0:#,0} items";
            const string StatusBarKB = " ({0:#,0} KB)";
            const string StatusBarMB = " ({0:#,0.0} MB)";
            const string StatusBarGB = " ({0:#,0.0} GB)";

            long bytes = _directoryController.DirectorySizeBytes;
            decimal kb = bytes / 1024.0m;
            decimal mb = kb / 1024.0m;
            decimal gb = mb / 1024.0m;

            int itemCount = _directoryController.FileCollection.Count;

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

            lblStatus.Text = text;
        }
    }
}
