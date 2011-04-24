using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Linq;
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

        /// <summary>
        /// Occurs when navigating starts.
        /// </summary>
        public event EventHandler Navigating;

        /// <summary>
        /// Occurs when navigation is complete.
        /// </summary>
        public event EventHandler NavigationComplete;

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
            GoUpCommand = new DelegateCommand(GoUp, () => true /* TODO */);
            SelectAllCommand = new DelegateCommand(SelectAll);
            InvertSelectionCommand = new DelegateCommand(InvertSelection);
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

        /// <summary>
        /// Gets the current directory.
        /// </summary>
        /// <value>The current directory.</value>
        public string CurrentDirectory
        {
            get { return _directory == null ? null : _directory.FullName; }
            set
            {
                NavigateTo(value, true);
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
                NavigateTo(parent.FullName, true);
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

            NavigateTo(goToItem.FullName, false);
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

            NavigateTo(goToItem.FullName, false);
        }

        /// <summary>
        /// Refreshes the view.
        /// </summary>
        public void RefreshExplorer()
        {
            // TODO: This may not cause a refresh
            NavigateTo(CurrentDirectory, false);
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
            SendEvent(Navigating);
            try
            {
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

                // Raise ListChanged event of ListChangedType.Reset
                //ResetBindings(); // TODO: this shouldn't be necessary with ObservableCollection

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

                SendPropertyChanged("CurrentDirectory", oldValue: oldDirectory, newValue: CurrentDirectory);
            }
            finally
            {
                SendEvent(NavigationComplete);
            }
        }

        private void SendEvent(EventHandler eventHandler)
        {
            if (eventHandler != null)
                eventHandler(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gets the directory size in bytes.
        /// </summary>
        /// <value>The directory size in bytes.</value>
        public long DirectorySizeBytes
        {
            get { return Get<long>(); }
            private set { Set(value); }
        }

        /// <summary>Clears the history.</summary>
        public void ClearHistory()
        {
            _forwardHistory.Clear();
            _backHistory.Clear();

            ((DelegateCommand)GoBackCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)GoForwardCommand).RaiseCanExecuteChanged();
        }

        /// <summary>Gets the file collection.</summary>
        public FileCollection FileCollection
        {
            get { return Get<FileCollection>(); }
            private set { Set(value); }
        }

        /// <summary>Gets the go back command.</summary>
        public ICommand GoBackCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        /// <summary>Gets the go forward command.</summary>
        public ICommand GoForwardCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        /// <summary>Gets the go up command.</summary>
        public ICommand GoUpCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        /// <summary>Gets the select all command.</summary>
        public ICommand SelectAllCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }

        /// <summary>Gets the invert selection command.</summary>
        public ICommand InvertSelectionCommand
        {
            get { return Get<ICommand>(); }
            private set { Set(value); }
        }
    }
}