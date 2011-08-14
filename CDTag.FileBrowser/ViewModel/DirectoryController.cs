using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using CDTag.Common;
using CDTag.FileBrowser.Model;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;

namespace CDTag.FileBrowser.ViewModel
{
    /// <summary>
    /// DirectoryController class. Used as a data source for file browsers.
    /// </summary>
    public class DirectoryController : ViewModelBase<IDirectoryController>, IDirectoryController
    {
        // TODO: Localize
        private const string _accessDeniedDialogTitle = "Access denied";

        private readonly BindingList<FileView> _backHistory = new BindingList<FileView>();
        private readonly BindingList<FileView> _forwardHistory = new BindingList<FileView>();
        private FileView _directory;
        private FileSystemWatcher _fsw;

        /// <summary>Occurs when navigating starts.</summary>
        public event EventHandler Navigating;

        /// <summary>Occurs when navigation is complete.</summary>
        public event EventHandler NavigationComplete;

        /// <summary>Occurs when a request is made to hide the address text box.</summary>
        public event EventHandler HideAddressTextBoxRequested;

        /// <summary>Occurs when a request is made to focus the address text box.</summary>
        public event EventHandler FocusAddressTextBoxRequested;

        /// <summary>Occurs when a request is made to close the popup.</summary>
        public event EventHandler ClosePopupRequested;

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryController"/> class.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        public DirectoryController(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            FileCollection = new FileCollection();

            GoBackCommand = new DelegateCommand(() => GoBack(), () => IsGoBackEnabled);
            GoForwardCommand = new DelegateCommand(() => GoForward(), () => IsGoForwardEnabled);
            GoUpCommand = new DelegateCommand(GoUp, () => CurrentDirectory != null && CurrentDirectory.Length > 3);
            SelectAllCommand = new DelegateCommand(SelectAll);
            InvertSelectionCommand = new DelegateCommand(InvertSelection);

            EnhancedPropertyChanged += DirectoryController_EnhancedPropertyChanged;

            History = new ObservableCollection<HistoryItem>();
        }

        private void DirectoryController_EnhancedPropertyChanged(object sender, EnhancedPropertyChangedEventArgs<IDirectoryController> e)
        {
            if (e.IsProperty(p => p.TypingDirectory))
            {
                //SubDirectories = new ObservableCollection<string>();
                GetSubDirectories(TypingDirectory);
            }
        }

        /// <summary>Selects all.</summary>
        private void SelectAll()
        {
            int totalCount = FileCollection.Count;
            int selectedCount = FileCollection.Count(p => p.IsSelected);

            bool value = (totalCount != selectedCount);
            foreach (var item in FileCollection)
            {
                item.IsSelected = value;
            }
        }

        /// <summary>Inverts the selection.</summary>
        private void InvertSelection()
        {
            foreach (var item in FileCollection)
            {
                item.IsSelected = !item.IsSelected;
            }
        }

        /// <summary>
        /// Gets the back history.
        /// </summary>
        /// <returns>The back history.</returns>
        public List<FileView> GetBackHistory()
        {
            return new List<FileView>(_backHistory);
        }

        /// <summary>
        /// Gets the forward history.
        /// </summary>
        /// <returns>The forward history.</returns>
        public List<FileView> GetForwardHistory()
        {
            return new List<FileView>(_forwardHistory);
        }

        /// <summary>Gets the current directory.</summary>
        /// <value>The current directory.</value>
        public string CurrentDirectory
        {
            get { return _directory == null ? null : _directory.FullName; }
            set
            {
                NavigateTo(directory: value, addHistory: true);
            }
        }

        /// <summary>Gets or sets the typing directory.</summary>
        /// <value>The typing directory.</value>
        public string TypingDirectory
        {
            get { return Get<string>(MethodBase.GetCurrentMethod()); }
            set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        /// <summary>Sets the initial directory.</summary>
        /// <value>The initial directory.</value>
        public string InitialDirectory
        {
            set
            {
                ClearHistory();
                NavigateTo(directory: value, addHistory: false);
            }
        }

        private bool IsGoBackEnabled
        {
            get { return _backHistory.Count > 0; }
        }

        private bool IsGoForwardEnabled
        {
            get { return _forwardHistory.Count > 0; }
        }

        private void GoUp()
        {
            DirectoryInfo di = new DirectoryInfo(_directory.FullName);
            DirectoryInfo parent = di.Parent;

            if (parent != null)
            {
                _forwardHistory.Clear();
                NavigateTo(directory: parent.FullName, addHistory: true);
            }
        }

        /// <summary>
        /// Go forward <paramref name="count"/> steps in the browsing history.
        /// </summary>
        /// <param name="count">The number of steps to go forward.</param>
        private void GoForward(int count = 1)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException("count", count, string.Format("Parameter 'count' must be >= 1."));

            if (_forwardHistory.Count == 0)
                return;

            if (_directory != null)
                _backHistory.Insert(0, _directory);

            int historyCount = _forwardHistory.Count;
            for (int i = 0; i < count - 1 && i < historyCount - 1; i++)
            {
                FileView item = _forwardHistory[0];

                _backHistory.Insert(0, item);
                _forwardHistory.RemoveAt(0);
            }

            FileView goToItem = _forwardHistory[0];
            _forwardHistory.RemoveAt(0);

            NavigateTo(directory: goToItem.FullName, addHistory: false);
        }

        private void GoBack(int count = 1)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException("count", count, string.Format("Parameter 'count' must be >= 1."));

            if (_backHistory.Count == 0)
                return;

            if (_directory != null)
                _forwardHistory.Insert(0, _directory);

            int historyCount = _backHistory.Count;
            for (int i = 0; i < count - 1 && i < historyCount - 1; i++)
            {
                FileView item = _backHistory[0];

                _forwardHistory.Insert(0, item);
                _backHistory.RemoveAt(0);
            }

            FileView goToItem = _backHistory[0];
            _backHistory.RemoveAt(0);

            NavigateTo(directory: goToItem.FullName, addHistory: false);
        }

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        public void RefreshExplorer()
        {
            // TODO: This may not cause a refresh
            NavigateTo(directory: CurrentDirectory, addHistory: false);
        }

        private static FileSystemInfo GetFileSystemInfo(string path)
        {
            // Return fileInfo for files and dirInfo for directories
            FileSystemInfo fileInfo = new FileInfo(path);

            if ((fileInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                fileInfo = new DirectoryInfo(path);
            }

            return fileInfo;
        }

        private void FileSystem_Changed(object sender, FileSystemEventArgs e)
        {
            FileView fv = FileCollection.Find(e.Name);

            if (null != fv)
            {
                long diff = 0 - fv.Size ?? 0;
                fv.Refresh();
                diff += fv.Size ?? 0;
                DirectorySizeBytes += diff;
                //ResetItem(IndexOf(fv));  // TODO: This shouldn't be necessary with ObservableCollection
            }
        }

        private void FileSystem_Created(object sender, FileSystemEventArgs e)
        {
            FileView fv = new FileView(GetFileSystemInfo(e.FullPath));
            DirectorySizeBytes += fv.Size ?? 0;
            FileCollection.Add(fv);
        }

        private void FileSystem_Deleted(object sender, FileSystemEventArgs e)
        {
            FileView fv = FileCollection.Find(e.Name);

            if (null != fv)
            {
                DirectorySizeBytes -= fv.Size ?? 0;
                FileCollection.Remove(fv);
            }
        }

        private void FileSystem_Renamed(object sender, RenamedEventArgs e)
        {
            FileView fv = FileCollection.Find(e.OldName);

            if (null != fv)
            {
                long diff = 0 - fv.Size ?? 0;
                fv.Refresh(GetFileSystemInfo(e.FullPath));
                diff += fv.Size ?? 0;
                DirectorySizeBytes += diff;
                //ResetItem(IndexOf(fv)); // TODO: This shouldn't be necessary with ObservableCollection
            }
        }

        /// <summary>Navigates to the specified <paramref name="directory"/>.</summary>
        /// <param name="directory">The directory.</param>
        /// <param name="addHistory">if set to <c>true</c> the previous directory will be added to the history.</param>
        private void NavigateTo(string directory, bool addHistory)
        {
            if (string.IsNullOrWhiteSpace(directory) || directory.Length < 3 || _isNavigating)
                return;

            if (string.Compare(directory, CurrentDirectory, true) == 0 ||
                string.Compare(directory, CurrentDirectory + @"\", true) == 0 ||
                string.Compare(directory + @"\", CurrentDirectory, true) == 0)
            {
                return;
            }

            // Uppercase drive letter
            if (char.IsLower(directory[0]))
            {
                directory = string.Format("{0}{1}", char.ToUpper(directory[0]), directory.Substring(1));
            }

            // Remove trailing slash
            if (directory.EndsWith(@"\") && directory.Length > 3)
            {
                directory = directory.Substring(0, directory.Length - 1);
            }

            SendNavigatingEvent();
            try
            {
                // Navigate up the tree until a valid directory is found
                while (!Directory.Exists(directory))
                {
                    DirectoryInfo directoryInfo = Directory.GetParent(directory);
                    if (directoryInfo != null)
                    {
                        directory = directoryInfo.FullName;
                    }
                    else
                    {
                        if (directory == @"C:\")
                        {
                            foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
                            {
                                if (driveInfo.IsReady)
                                {
                                    directory = driveInfo.RootDirectory.FullName;
                                    break;
                                }
                            }
                            break;
                        }
                        else
                        {
                            directory = @"C:\";
                        }
                    }
                }

                // Fix capitalization
                string shortDirName = Path.GetFileName(directory);
                DirectoryInfo dirInfo = new DirectoryInfo(directory);
                if (dirInfo.Parent != null)
                {
                    dirInfo = dirInfo.Parent.GetDirectories(shortDirName)[0];
                    directory = dirInfo.FullName;
                }

                if (_backHistory.Count > 0)
                {
                    if (string.Compare(_backHistory[0].FullName, directory, true) == 0)
                    {
                        _backHistory.RemoveAt(0);
                    }
                }

                if (_directory != null && string.Compare(_directory.FullName, directory, true) == 0)
                    addHistory = false;

                DirectoryInfo info;
                DirectoryInfo[] diArray;
                FileInfo[] fiArray;

                try
                {
                    info = new DirectoryInfo(directory);
                    diArray = info.GetDirectories();
                    fiArray = info.GetFiles();

                    Array.Sort(diArray.Select(p => p.Name).ToArray(), diArray, StringComparer.CurrentCultureIgnoreCase);
                    Array.Sort(fiArray.Select(p => p.Name).ToArray(), fiArray, StringComparer.CurrentCultureIgnoreCase);
                }
                catch (UnauthorizedAccessException ex)
                {
                    Mouse.OverrideCursor = null;
                    MessageBox.Show(ex.Message, _accessDeniedDialogTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                FileCollection fileCollection = new FileCollection();
                long directorySize = 0;
                // Load child files and directories
                foreach (DirectoryInfo di in diArray)
                {
                    if ((di.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                        fileCollection.Add(new FileView(di));
                }

                foreach (FileInfo fi in fiArray)
                {
                    if ((fi.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                    {
                        FileView fv = new FileView(fi);
                        directorySize += fv.Size ?? 0;
                        fileCollection.Add(fv);
                    }
                }

                FileCollection = fileCollection;
                DirectorySizeBytes = directorySize;

                if (addHistory)
                {
                    if (_directory != null)
                        _backHistory.Insert(0, _directory);
                    _forwardHistory.Clear();
                }

                string oldDirectory = CurrentDirectory;
                _directory = new FileView(info);

                // Setup the FileSystemWatcher
                if (_fsw != null)
                {
                    _fsw.Changed -= FileSystem_Changed;
                    _fsw.Created -= FileSystem_Created;
                    _fsw.Deleted -= FileSystem_Deleted;
                    _fsw.Renamed -= FileSystem_Renamed;
                }

                _fsw = new FileSystemWatcher(directory);
                _fsw.EnableRaisingEvents = true;

                _fsw.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.LastAccess;
                _fsw.Changed += FileSystem_Changed;
                _fsw.Created += FileSystem_Created;
                _fsw.Deleted += FileSystem_Deleted;
                _fsw.Renamed += FileSystem_Renamed;

                ((DelegateCommand)GoBackCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)GoForwardCommand).RaiseCanExecuteChanged();
                ((DelegateCommand)GoUpCommand).RaiseCanExecuteChanged();

                SendPropertyChanged("CurrentDirectory", oldValue: oldDirectory, newValue: CurrentDirectory);
            }
            finally
            {
                SendNavigationCompleteEvent();
            }
        }

        private bool _isNavigating;
        private void SendNavigatingEvent()
        {
            _isNavigating = true;
            var eventHandler = Navigating;
            if (eventHandler != null)
                eventHandler(this, EventArgs.Empty);
        }

        private void SendNavigationCompleteEvent()
        {
            TypingDirectory = CurrentDirectory;
            UpdateHistory();

            _isNavigating = false;
            var eventHandler = NavigationComplete;
            if (eventHandler != null)
                eventHandler(this, EventArgs.Empty);
        }

        private void UpdateHistory()
        {
            // TODO: This could be more efficient. It may make more sense to have back/forward history derive from this list rather than the way it is now.

            ObservableCollection<HistoryItem> history = new ObservableCollection<HistoryItem>();

            int forwardCount = _forwardHistory.Count;
            foreach (var item in _forwardHistory.Reverse())
                history.Add(new HistoryItem { FileView = item, HistoryOffset = forwardCount-- });

            history.Add(new HistoryItem { FileView = _directory, IsCurrent = true, HistoryOffset = 0 });

            int historyOffset = -1;
            foreach (var item in _backHistory)
                history.Add(new HistoryItem { FileView = item, HistoryOffset = historyOffset-- });

            History = history;
        }

        private void GetSubDirectories(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory) || directory.Length < 3)
            {
                if (SubDirectories != null)
                    SubDirectories.Clear();
                return;
            }

            Thread thread = new Thread(GetSubDirectoriesAsync);
            thread.Start(directory);
        }

        private void GetSubDirectoriesAsync(object baseDirectory)
        {
            try
            {
                string directory = (string)baseDirectory;
                directory = char.ToUpper(directory[0]) + directory.Substring(1);
                string origDirectory = directory;
                string searchPattern = "*";
                if (!Directory.Exists(directory) || !directory.EndsWith(@"\"))
                {
                    searchPattern = Path.GetFileName(directory) + "*";
                    directory = Path.GetDirectoryName(directory);
                    if (!Directory.Exists(directory))
                        return;
                }

                string[] dirs = Directory.GetDirectories(directory, searchPattern);
                if (dirs.Length == 1 && string.Compare(dirs[0], origDirectory, ignoreCase: true) == 0)
                {
                    GetSubDirectoriesAsync(origDirectory + @"\");
                    return;
                }

                ObservableCollection<string> subDirs = new ObservableCollection<string>();
                foreach (string dir in dirs)
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(dir);
                    if (!directoryInfo.Attributes.HasFlag(FileAttributes.Hidden))
                        subDirs.Add(directoryInfo.FullName);
                }

                Application.Current.Dispatcher.Invoke(new Action(() => SubDirectories = subDirs));
            }
            catch
            {
                // TODO ?
            }
        }

        /// <summary>
        /// Gets the directory size in bytes.
        /// </summary>
        /// <value>The directory size in bytes.</value>
        public long DirectorySizeBytes
        {
            get { return Get<long>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        /// <summary>Clears the history.</summary>
        public void ClearHistory()
        {
            _forwardHistory.Clear();
            _backHistory.Clear();

            ((DelegateCommand)GoBackCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)GoForwardCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)GoUpCommand).RaiseCanExecuteChanged();
        }

        /// <summary>Gets the file collection.</summary>
        public FileCollection FileCollection
        {
            get { return Get<FileCollection>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        /// <summary>Gets the go back command.</summary>
        public ICommand GoBackCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        /// <summary>Gets the go forward command.</summary>
        public ICommand GoForwardCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        /// <summary>Gets the go up command.</summary>
        public ICommand GoUpCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        /// <summary>Gets the select all command.</summary>
        public ICommand SelectAllCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        /// <summary>Gets the invert selection command.</summary>
        public ICommand InvertSelectionCommand
        {
            get { return Get<ICommand>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        /// <summary>Gets the selected items.</summary>
        public List<FileView> SelectedItems
        {
            get
            {
                return FileCollection.Where(p => p.IsSelected).ToList();
            }
        }

        /// <summary>
        /// Gets the sub directories of the <see cref="CurrentDirectory"/>.
        /// </summary>
        public ObservableCollection<string> SubDirectories
        {
            get { return Get<ObservableCollection<string>>(MethodBase.GetCurrentMethod()); }
            set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        /// <summary>Gets the history.</summary>
        public ObservableCollection<HistoryItem> History
        {
            get { return Get<ObservableCollection<HistoryItem>>(MethodBase.GetCurrentMethod()); }
            set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        /// <summary>Navigates to the specified history offset.</summary>
        /// <param name="offset">The history offset.</param>
        public void Navigate(int offset)
        {
            if (offset == 0)
                return;

            if (offset < 0)
                GoBack(offset * -1);
            else
                GoForward(offset);
        }

        /// <summary>Focuses the address text box.</summary>
        public void FocusAddressTextBox()
        {
            var handler = FocusAddressTextBoxRequested;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>Hides the address text box.</summary>
        public void HideAddressTextBox()
        {
            var handler = HideAddressTextBoxRequested;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        /// <summary>Closes the popup.</summary>
        public void ClosePopup()
        {
            var handler = ClosePopupRequested;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}