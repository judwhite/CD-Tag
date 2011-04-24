using System;
using System.Windows.Controls;
using System.Windows.Input;
using CDTag.Common;
using CDTag.FileBrowser.Model;
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

            FolderTreeView.NavigationComplete += treeView1_NavigationComplete;
            FileList.fileView.SelectionChanged += fileView_SelectionChanged;

            _directoryController.NavigationComplete += _directoryController_NavigationComplete;
            _directoryController.SelectAllRequested += (o, e) => SelectAll();
            _directoryController.InvertSelectionRequested += (o, e) => InvertSelection();

            PreviewMouseDown += FileExplorer_PreviewMouseDown;
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
        public IDirectoryController DirectoryController
        {
            get { return _directoryController; }
        }

        private void treeView1_NavigationComplete(object sender, EventArgs e)
        {
            MouseHelper.SetWaitCursor();
            try
            {
                _directoryController.NavigateTo(FolderTreeView.CurrentDirectory);
            }
            finally
            {
                MouseHelper.ResetCursor();
            }
        }

        /// <summary>Selects all.</summary>
        public void SelectAll()
        {
            if (FileList.fileView.SelectedItems.Count == FileList.fileView.Items.Count)
            {
                FileList.fileView.UnselectAll();
            }
            else
            {
                FileList.fileView.SelectAll();
            }
        }

        /// <summary>Inverts the selection.</summary>
        public void InvertSelection()
        {
            var selectedItems = FileList.fileView.SelectedItems;

            foreach (var item in FileList.fileView.Items)
            {
                int index = selectedItems.IndexOf(item);
                if (index != -1)
                    selectedItems.RemoveAt(index);
                else
                    selectedItems.Add(item);
            }
        }

        /// <summary>Goes back.</summary>
        public void GoBack()
        {
            MouseHelper.SetWaitCursor();

            _directoryController.GoBack();

            MouseHelper.ResetCursor();
        }

        /// <summary>Goes forward.</summary>
        public void GoForward()
        {
            MouseHelper.SetWaitCursor();

            _directoryController.GoForward();

            MouseHelper.ResetCursor();
        }

        /// <summary>Goes up.</summary>
        public void GoUp()
        {
            MouseHelper.SetWaitCursor();

            _directoryController.GoUp();

            MouseHelper.ResetCursor();
        }

        private void fileView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // TODO
            foreach (object o in e.AddedItems)
            {
            }
        }

        private void _directoryController_NavigationComplete(object sender, EventArgs e)
        {
            MouseHelper.SetWaitCursor();

            FolderTreeView.NavigateTo(_directoryController.CurrentDirectory);
            FileList.UpdateStatusBar();

            MouseHelper.ResetCursor();
        }
    }
}
