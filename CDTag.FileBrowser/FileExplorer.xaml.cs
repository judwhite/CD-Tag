using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace CDTag.FileBrowser
{
    /// <summary>
    /// Interaction logic for FileExplorer.xaml
    /// </summary>
    public partial class FileExplorer : UserControl
    {
        private readonly DirectoryController _directoryController = new DirectoryController();
        private int _mouseCursorWaitCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileExplorer"/> class.
        /// </summary>
        public FileExplorer()
        {
            InitializeComponent();

            FolderTreeView.NavigationComplete += treeView1_NavigationComplete;
            FileList.fileView.SelectionChanged += fileView_SelectionChanged;
            FileList.fileView.MouseDoubleClick += fileView_MouseDoubleClick;

            _directoryController.NavigationComplete += _directoryController_NavigationComplete;

            PreviewMouseDown += FileExplorer_PreviewMouseDown;
        }

        private void FileExplorer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.XButton1 == MouseButtonState.Pressed)
            {
                if (_directoryController.IsGoBackEnabled)
                {
                    GoBack();
                    e.Handled = true;
                }
            }
            else if (e.XButton2 == MouseButtonState.Pressed)
            {
                if (_directoryController.IsGoForwardEnabled)
                {
                    GoForward();
                    e.Handled = true;
                }
            }
        }

        /// <summary>Gets the directory controller.</summary>
        public DirectoryController DirectoryController
        {
            get { return _directoryController; }
        }

        private void treeView1_NavigationComplete(object sender, EventArgs e)
        {
            SetWaitCursor();

            _directoryController.NavigateTo(FolderTreeView.CurrentDirectory);

            ResetCursor();
        }

        /// <summary>Goes back.</summary>
        public void GoBack()
        {
            SetWaitCursor();

            _directoryController.GoBack();

            ResetCursor();
        }

        /// <summary>Goes forward.</summary>
        public void GoForward()
        {
            SetWaitCursor();

            _directoryController.GoForward();

            ResetCursor();
        }

        /// <summary>Goes up.</summary>
        public void GoUp()
        {
            SetWaitCursor();

            _directoryController.GoUp();

            ResetCursor();
        }

        private void SetWaitCursor()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            _mouseCursorWaitCount++;
        }

        private void ResetCursor()
        {
            _mouseCursorWaitCount--;
            if (_mouseCursorWaitCount <= 0)
            {
                _mouseCursorWaitCount = 0;
                Mouse.OverrideCursor = null;
            }
        }

        private void fileView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // TODO
            foreach (object o in e.AddedItems)
            {
            }
        }

        private void fileView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (FileList.fileView.SelectedItems.Count == 1)
            {
                FileView item = FileList.fileView.SelectedItem as FileView;
                if (item == null)
                    return;

                if (item.IsDirectory)
                {
                    SetWaitCursor();

                    _directoryController.NavigateTo(item.FullName);

                    ResetCursor();
                }
            }
        }

        private void _directoryController_NavigationComplete(object sender, EventArgs e)
        {
            SetWaitCursor();

            if (FileList.fileView.ItemsSource == null)
                FileList.fileView.ItemsSource = _directoryController;

            FolderTreeView.NavigateTo(_directoryController.CurrentDirectory);
            FileList.UpdateStatusBar(_directoryController);

            ResetCursor();
        }
    }
}
