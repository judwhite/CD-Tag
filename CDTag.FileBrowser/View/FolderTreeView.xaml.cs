using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CDTag.Common.Win32API;
using CDTag.FileBrowser.Model;

namespace CDTag.FileBrowser.View
{
    /// <summary>
    /// Interaction logic for FolderTreeView.xaml
    /// </summary>
    public partial class FolderTreeView : UserControl
    {
        // TODO: Get string values from a resource file or make them public
        private const string DriveTypes_LocalDisk = "Local Disk";
        private const string DriveTypes_CDDVD = "CD/DVD ({0})";
        private const string DriveTypes_RemovableDisk = "Removable Disk ({0})";
        private const string DriveNotReadyDialogTitle = "Drive not ready";
        private const string DriveNotReadyMessage = "Drive {0} not ready.";
        private const string AccessDeniedDialogTitle = "Access denied";

        private bool _drivesAdded;

        /// <summary>
        /// Initializes a new instance of the <see cref="FolderTreeView"/> class.
        /// </summary>
        public FolderTreeView()
        {
            InitializeComponent();

            treeView1.SelectedItemChanged += treeView1_SelectedItemChanged;
        }

        private bool _suppressEventHandling;
        private string _currentDirectory;
        private bool _navigating;
        private int _updateCount;

        /// <summary>
        /// Occurs when navigation is complete.
        /// </summary>
        public event EventHandler NavigationComplete;

        private class DirectoryTreeNode : TreeViewItem
        {
            private string _directory;

            public bool IsAccessDenied { get; private set; }
            public bool IsDrive { get; private set; }
            public string AccessDeniedMessage { get; private set; }

            public string Directory
            {
                get { return _directory; }
                set
                {
                    _directory = value;

                    Win32.SHFILEINFO info = new Win32.SHFILEINFO();
                    Win32.SHGetFileInfo(_directory, 0, ref info, (uint)Marshal.SizeOf(info), Win32.SHGFI_ICON | Win32.SHGFI_TYPENAME | Win32.SHGFI_SMALLICON);

                    ImageSource img;
                    using (System.Drawing.Icon icon = System.Drawing.Icon.FromHandle(info.hIcon))
                    {
                        img = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, new Int32Rect(0, 0, icon.Width, icon.Height), BitmapSizeOptions.FromEmptyOptions());
                    }

                    string text = Path.GetFileName(_directory);

                    if (string.IsNullOrEmpty(text))
                    {
                        IsDrive = true;

                        DriveInfo drive = new DriveInfo(_directory);

                        string volumeLabel = string.Empty;
                        if (drive.IsReady)
                            volumeLabel = drive.VolumeLabel;
                        string driveLetter = drive.Name.TrimEnd('\\');

                        switch (drive.DriveType)
                        {
                            case DriveType.Fixed:
                                if (string.IsNullOrEmpty(volumeLabel))
                                    volumeLabel = DriveTypes_LocalDisk;
                                break;
                            case DriveType.CDRom:
                                text = string.Format(DriveTypes_CDDVD, driveLetter);
                                break;
                            case DriveType.Removable:
                                text = string.Format(DriveTypes_RemovableDisk, driveLetter);
                                break;
                            default:
                                text = string.Format("{0}", driveLetter);
                                break;
                        }

                        if (drive.DriveType == DriveType.Fixed)
                        {
                            text = string.Format("{0} ({1})", volumeLabel, driveLetter);
                        }
                        else if (!string.IsNullOrEmpty(volumeLabel))
                        {
                            text += " " + volumeLabel;
                        }

                        if (drive.IsReady)
                        {
                            if (System.IO.Directory.GetDirectories(_directory).Length != 0)
                            {
                                Items.Add(null);
                            }
                        }
                    }
                    else
                    {
                        string[] dirs;
                        try
                        {
                            dirs = System.IO.Directory.GetDirectories(_directory);
                            if (dirs.Length != 0)
                            {
                                Items.Add(null);
                            }
                        }
                        catch (Exception ex)
                        {
                            IsAccessDenied = true;
                            AccessDeniedMessage = ex.Message;
                        }
                    }

                    Header = BuildHeader(img, text);
                }
            }

            private static StackPanel BuildHeader(ImageSource img, string text)
            {
                StackPanel stack = new StackPanel();
                stack.Orientation = Orientation.Horizontal;

                Image icon = new Image();
                icon.VerticalAlignment = VerticalAlignment.Center;
                icon.Margin = new Thickness(0, 0, 4, 0);
                icon.Source = img;
                stack.Children.Add(icon);

                TextBlock textBlock = new TextBlock();
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                textBlock.Text = text;
                stack.Children.Add(textBlock);

                return stack;
            }

            public static readonly RoutedEvent CollapsingEvent = EventManager.RegisterRoutedEvent("Collapsing", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DirectoryTreeNode));

            public static readonly RoutedEvent ExpandingEvent = EventManager.RegisterRoutedEvent("Expanding", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DirectoryTreeNode));

            public event RoutedEventHandler Collapsing
            {
                add { AddHandler(CollapsingEvent, value); }
                remove { RemoveHandler(CollapsingEvent, value); }
            }

            public event RoutedEventHandler Expanding
            {
                add { AddHandler(ExpandingEvent, value); }
                remove { RemoveHandler(ExpandingEvent, value); }
            }

            protected override void OnExpanded(RoutedEventArgs e)
            {
                OnExpanding(new RoutedEventArgs(ExpandingEvent, this));
                base.OnExpanded(e);
            }

            protected override void OnCollapsed(RoutedEventArgs e)
            {
                OnCollapsing(new RoutedEventArgs(CollapsingEvent, this));
                base.OnCollapsed(e);
            }

            protected virtual void OnCollapsing(RoutedEventArgs e) { RaiseEvent(e); }
            protected virtual void OnExpanding(RoutedEventArgs e) { RaiseEvent(e); }
        }

        /// <summary>
        /// Navigates to a specified directory.
        /// </summary>
        /// <param name="fileView">The directory.</param>
        public void NavigateTo(FileView fileView)
        {
            if (fileView == null)
                throw new ArgumentNullException("fileView");

            _navigating = true;
            try
            {
                if (!_drivesAdded)
                {
                    _drivesAdded = true;

                    foreach (DriveInfo drive in DriveInfo.GetDrives())
                    {
                        DirectoryTreeNode node = new DirectoryTreeNode();
                        node.Expanding += treeView1_BeforeExpand;
                        node.Collapsing += treeView1_BeforeCollapse;
                        treeView1.Items.Add(node);

                        node.Directory = drive.RootDirectory.FullName;
                    }
                }

                if (!fileView.IsDirectory)
                    return;

                if (string.Compare(CurrentDirectory, fileView.FullName, true) == 0)
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
            finally
            {
                _navigating = false;
            }

            SendEvent(NavigationComplete);
        }

        private TreeViewItem FindNode(TreeViewItem parentNode, ItemCollection nodes, string directory)
        {
            return FindNode(parentNode, nodes, directory, 1);
        }

        private TreeViewItem FindNode(TreeViewItem parentNode, ItemCollection nodes, string directory, int attemptNumber)
        {
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
        /// Gets the current directory.
        /// </summary>
        /// <value>The current directory.</value>
        public string CurrentDirectory
        {
            get { return _currentDirectory; }
            private set
            {
                _currentDirectory = value;
                if (!_suppressEventHandling)
                    SendEvent(NavigationComplete);
            }
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

            if (CurrentDirectory != node.Directory)
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
                    CurrentDirectory = node.Directory;
                }
            }

            SendEvent(NavigationComplete);

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

            SendEvent(NavigationComplete);

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
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    node.Items.Clear();
                    foreach (DirectoryInfo dir in dirs.OrderBy(p => p.FullName))
                    {
                        if ((dir.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                        {
                            DirectoryTreeNode childNode = new DirectoryTreeNode();
                            childNode.Expanding += treeView1_BeforeExpand;
                            childNode.Collapsing += treeView1_BeforeCollapse;
                            node.Items.Add(childNode);
                            childNode.Directory = dir.FullName;
                        }
                    }

                    CurrentDirectory = node.Directory;
                    if (!_suppressEventHandling)
                        node.IsSelected = true;
                }
                finally
                {
                    Mouse.OverrideCursor = null;
                    TreeViewEndUpdate();
                }
            }
            else
            {
                MessageBox.Show(node.AccessDeniedMessage, AccessDeniedDialogTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }

            SendEvent(NavigationComplete);
        }

        private void TreeViewBeginUpdate()
        {
            _updateCount++;
        }

        private void TreeViewEndUpdate()
        {
            _updateCount--;
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
                CurrentDirectory = node.Directory;
                node.IsSelected = true;
                node.IsExpanded = false;
            }
            finally
            {
                _suppressEventHandling = false;
            }

            SendEvent(NavigationComplete);
        }

        private void SendEvent(EventHandler eventHandler)
        {
            if (!_navigating)
            {
                if (eventHandler != null)
                    eventHandler(this, EventArgs.Empty);
            }
        }
    }
}