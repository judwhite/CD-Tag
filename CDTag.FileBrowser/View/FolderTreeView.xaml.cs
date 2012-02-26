using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CDTag.Common;
using CDTag.Common.Events;
using CDTag.Common.Wpf;
using CDTag.FileBrowser.Model;
using CDTag.FileBrowser.ViewModel;

namespace CDTag.FileBrowser.View
{
    /// <summary>
    /// Interaction logic for FolderTreeView.xaml
    /// </summary>
    public partial class FolderTreeView : UserControl
    {
        private IDirectoryController _viewModel;

        // TODO: Get string values from a resource file or make them public
        private const string DriveNotReadyDialogTitle = "Drive not ready";
        private const string DriveNotReadyMessage = "Drive {0} not ready.";
        private const string AccessDeniedDialogTitle = "Access denied";

        private bool _drivesAdded;

        private bool _suppressEventHandling;
        private int _updateCount;

        private string _oldDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderTreeView"/> class.
        /// </summary>
        public FolderTreeView()
        {
            InitializeComponent();

            DataContextChanged += FolderTreeView_DataContextChanged;
            treeView1.SelectedItemChanged += treeView1_SelectedItemChanged;
        }

        private void FolderTreeView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _viewModel = (IDirectoryController)DataContext;
            _viewModel.EnhancedPropertyChanged += _viewModel_EnhancedPropertyChanged;
        }

        private void _viewModel_EnhancedPropertyChanged(object sender, EnhancedPropertyChangedEventArgs<IDirectoryController> e)
        {
            if (e.IsProperty(p => p.CurrentDirectory))
            {
                NavigateTo(_viewModel.CurrentDirectory);
            }
        }

        /// <summary>Navigates to a specified directory.</summary>
        /// <param name="directory">The directory.</param>
        private void NavigateTo(string directory)
        {
            if (string.IsNullOrEmpty(directory))
                throw new ArgumentNullException("directory");

            FileView fileView = new FileView(directory);
            string currentDirectory = _oldDirectory;
            _oldDirectory = fileView.FullName;

            if (!_drivesAdded)
            {
                _drivesAdded = true;

                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    DirectoryTreeNode node = new DirectoryTreeNode(0);
                    node.Expanding += treeView1_BeforeExpand;
                    node.Collapsing += treeView1_BeforeCollapse;
                    treeView1.Items.Add(node);

                    node.Directory = drive.RootDirectory.FullName;
                }
            }

            if (!fileView.IsDirectory)
                return;

            if (string.Compare(currentDirectory, fileView.FullName, true) == 0)
                return;

            List<string> nodeNames = new List<string>();
            nodeNames.Add(fileView.FullName);
            string upLevel = Path.GetDirectoryName(fileView.FullName);
            while (!string.IsNullOrEmpty(upLevel))
            {
                nodeNames.Add(upLevel);
                upLevel = Path.GetDirectoryName(upLevel);
            }

            ItemCollection nodes = treeView1.Items;
            TreeViewItem parentNode = null;

            TreeViewBeginUpdate();
            _suppressEventHandling = true;
            try
            {
                for (int i = nodeNames.Count - 1; i >= 0; i--)
                {
                    parentNode = FindNode(parentNode, nodes, nodeNames[i]);
                    nodes = parentNode.Items;
                }

                if (parentNode != null)
                {
                    UpdateNodeChildren((DirectoryTreeNode)parentNode);
                    parentNode.IsSelected = true;
                    parentNode.IsExpanded = true;
                    parentNode.BringIntoView();
                }
            }
            finally
            {
                _suppressEventHandling = false;
                TreeViewEndUpdate();
            }
        }

        private TreeViewItem FindNode(TreeViewItem parentNode, ItemCollection nodes, string directory, int attemptNumber = 1)
        {
            if (attemptNumber < 1)
                throw new ArgumentOutOfRangeException("attemptNumber", attemptNumber, "attemptNumber must be >= 1");

            if (attemptNumber > 2)
            {
                return parentNode;
            }

            List<TreeViewItem> childNodes = new List<TreeViewItem>();
            foreach (TreeViewItem childNode in nodes)
            {
                childNodes.Add(childNode);
                DirectoryTreeNode dirNode = childNode as DirectoryTreeNode;
                if (dirNode == null)
                    continue;

                if (string.Compare(dirNode.Directory, directory, true) == 0)
                {
                    return dirNode;
                }
            }

            UpdateNodeChildren((DirectoryTreeNode)parentNode);

            TreeViewItem node = FindNode(parentNode, parentNode.Items, directory, attemptNumber + 1);
            return node;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the 'Folders' header.
        /// </summary>
        /// <value><c>true</c> if the 'Folders' header should be shown; otherwise, <c>false</c>.</value>
        [Browsable(true)]
        public bool ShowHeader
        {
            get { return panel1.Visibility == Visibility.Visible; }
            set { panel1.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }

        private void treeView1_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (_suppressEventHandling)
                return;

            DirectoryTreeNode node = e.NewValue as DirectoryTreeNode;
            if (node == null)
                return;

            string currentDirectory = _viewModel.CurrentDirectory;

            if (currentDirectory != node.Directory)
            {
                if (!node.IsExpanded)
                {
                    _suppressEventHandling = true;
                    try
                    {
                        UpdateNodeChildren(node);
                        node.IsExpanded = true;
                    }
                    finally
                    {
                        _suppressEventHandling = false;
                    }
                }
                else
                {
                    _viewModel.CurrentDirectory = node.Directory;
                }
            }

            e.Handled = true;
        }

        private void treeView1_BeforeExpand(object sender, RoutedEventArgs e)
        {
            if (_suppressEventHandling)
                return;

            DirectoryTreeNode node = sender as DirectoryTreeNode;
            if (node == null)
                return;

            _suppressEventHandling = true;
            try
            {
                UpdateNodeChildren(node);
                node.IsSelected = true;
            }
            finally
            {
                _suppressEventHandling = false;
            }

            e.Handled = true;
        }

        private void UpdateNodeChildren(DirectoryTreeNode node)
        {
            if (!node.IsAccessDenied)
            {
                if (node.IsDrive)
                {
                    DriveInfo driveInfo = new DriveInfo(node.Directory);
                    if (!driveInfo.IsReady)
                    {
                        node.Items.Clear();
                        MessageBox.Show(string.Format(DriveNotReadyMessage, node.Directory), DriveNotReadyDialogTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }

                DirectoryInfo info = new DirectoryInfo(node.Directory);
                DirectoryInfo[] dirs = info.GetDirectories();

                TreeViewBeginUpdate();
                try
                {
                    node.Items.Clear();
                    foreach (DirectoryInfo dir in dirs.OrderBy(p => p.FullName))
                    {
                        if ((dir.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                        {
                            DirectoryTreeNode childNode = new DirectoryTreeNode(node.Depth + 1);
                            childNode.Expanding += treeView1_BeforeExpand;
                            childNode.Collapsing += treeView1_BeforeCollapse;
                            node.Items.Add(childNode);
                            childNode.Directory = dir.FullName;
                        }
                    }

                    _viewModel.CurrentDirectory = node.Directory;
                    if (!_suppressEventHandling)
                        node.IsSelected = true;
                }
                finally
                {
                    TreeViewEndUpdate();
                }
            }
            else
            {
                MessageBox.Show(node.AccessDeniedMessage, AccessDeniedDialogTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void TreeViewBeginUpdate()
        {
            _updateCount++;
            MouseHelper.SetWaitCursor();
        }

        private void TreeViewEndUpdate()
        {
            _updateCount--;
            if (_updateCount <= 0)
                _updateCount = 0;
            MouseHelper.ResetCursor();
        }

        private void treeView1_BeforeCollapse(object sender, RoutedEventArgs e)
        {
            if (_suppressEventHandling)
                return;

            DirectoryTreeNode node = sender as DirectoryTreeNode;
            if (node == null)
                return;

            _suppressEventHandling = true;
            try
            {
                _viewModel.CurrentDirectory = node.Directory;
                node.IsSelected = true;
                node.IsExpanded = false;
            }
            finally
            {
                _suppressEventHandling = false;
            }
        }
    }
}