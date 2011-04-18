using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CDTag.FileBrowser
{
	/// <summary>
	/// DirectoryController class. Used as a data source for file browsers.
	/// </summary>
	public class DirectoryController : BindingList<FileView>, IDirectoryController
	{
		private readonly AsyncOperation _oper;
		private readonly BindingList<FileView> _backHistory = new BindingList<FileView>();
		private readonly BindingList<FileView> _forwardHistory = new BindingList<FileView>();
		private FileView _directory;
		private bool _suspend;
		private FileSystemWatcher _fsw;
		private long _directorySize;

		/// <summary>
		/// Occurs when <see cref="DirectorySizeBytes"/> has changed.
		/// </summary>
		public event EventHandler DirectorySizeChanged;

		/// <summary>
		/// Occurs when navigating starts.
		/// </summary>
		public event EventHandler Navigating;

		/// <summary>
		/// Occurs when navigation is complete.
		/// </summary>
		public event EventHandler NavigationComplete;

		/// <summary>
		/// Occurs when <see cref="IsGoBackEnabled"/> has changed.
		/// </summary>
		public event EventHandler GoBackEnabledChanged;

		/// <summary>
		/// Occurs when <see cref="IsGoForwardEnabled"/> has changed.
		/// </summary>
		public event EventHandler GoForwardEnabledChanged;

		/// <summary>
		/// Initializes a new instance of the <see cref="DirectoryController"/> class, setting the initial directory to the <see cref="Environment.SpecialFolder.Personal"/> directory.
		/// </summary>
		public DirectoryController() :
			this(Environment.GetFolderPath(Environment.SpecialFolder.Personal))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DirectoryController"/> class.
		/// </summary>
		/// <param name="directory">The initial directory.</param>
        public DirectoryController(string directory)
		{
			// Setup Async
			_oper = AsyncOperationManager.CreateOperation(null);

			NavigateTo(directory);
		}

		/// <summary>
		/// Raises the <see cref="E:System.ComponentModel.BindingList`1.ListChanged"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.ComponentModel.ListChangedEventArgs"/> that contains the event data.</param>
		protected override void OnListChanged(ListChangedEventArgs e)
		{
			//base.OnListChanged(e);
			if (!_suspend)
			{
				// Debug Info
				//WriteDebugThreadInfo("OnListChanged");

				// We need to marshall changes back to the UI thread since FileSystemWatcher fires
				// updates on any ol' thread.
				_oper.Post(PostCallback, e);
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
		/// Navigates to the specified <paramref name="directory"/>.
		/// </summary>
		/// <param name="directory">The directory.</param>
		public void NavigateTo(string directory)
		{
			NavigateTo(directory, true);
		}

		/// <summary>
		/// Gets the current directory.
		/// </summary>
		/// <value>The current directory.</value>
		public FileView CurrentDirectory
		{
			get { return _directory; }
		}

		/// <summary>
		/// Gets a value indicating whether GoBack should be enabled.
		/// </summary>
		/// <value><c>true</c> if GoBack should be enabled; otherwise, <c>false</c>.</value>
		public bool IsGoBackEnabled
		{
			get { return _backHistory.Count > 0; }
		}

		/// <summary>
		/// Gets a value indicating whether GoForward should be enabled.
		/// </summary>
		/// <value><c>true</c> if GoForward should be enabled; otherwise, <c>false</c>.</value>
		public bool IsGoForwardEnabled
		{
			get { return _forwardHistory.Count > 0; }
		}

		/// <summary>
		/// Go up a level.
		/// </summary>
		public void GoUp()
		{
			DirectoryInfo di = new DirectoryInfo(_directory.FullName);
			DirectoryInfo parent = di.Parent;

			if (parent != null)
			{
				_forwardHistory.Clear();
				NavigateTo(parent.FullName);
			}
		}

		/// <summary>
		/// Go forward in the browsing history.
		/// </summary>
		public void GoForward()
		{
			GoForward(1);
		}

		/// <summary>
		/// Go forward <paramref name="count"/> steps in the browsing history.
		/// </summary>
		/// <param name="count">The number of steps to go forward.</param>
		public void GoForward(int count)
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

		/// <summary>
		/// Go back in the browsing history.
		/// </summary>
		public void GoBack()
		{
			GoBack(1);
		}

		/// <summary>
		/// Go back <paramref name="count"/> steps in the browsing history.
		/// </summary>
		/// <param name="count">The number of steps to go back.</param>
		public void GoBack(int count)
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
			NavigateTo(CurrentDirectory.FullName, false);
		}

		private FileView Find(string name)
		{
			FileView item = null;

			foreach (FileView fileView in Items)
			{
				if (fileView.Name == name)
				{
					item = fileView;
					break;
				}
			}

			return item;
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
			FileView fv = Find(e.Name);

			if (null != fv)
			{
				long diff = 0 - fv.Size ?? 0;
				fv.Refresh();
				diff += fv.Size ?? 0;
				DirectorySizeBytes += diff;
				ResetItem(IndexOf(fv));
			}
		}

		private void FileSystem_Created(object sender, FileSystemEventArgs e)
		{
			FileView fv = new FileView(GetFileSystemInfo(e.FullPath));
			DirectorySizeBytes += fv.Size ?? 0;
			Add(fv);
		}

		private void FileSystem_Deleted(object sender, FileSystemEventArgs e)
		{
			FileView fv = Find(e.Name);

			if (null != fv)
			{
				DirectorySizeBytes -= fv.Size ?? 0;
				Remove(fv);
			}
		}

		private void FileSystem_Renamed(object sender, RenamedEventArgs e)
		{
			FileView fv = Find(e.OldName);

			if (null != fv)
			{
				long diff = 0 - fv.Size ?? 0;
				fv.Refresh(GetFileSystemInfo(e.FullPath));
				diff += fv.Size ?? 0;
				DirectorySizeBytes += diff;
				ResetItem(IndexOf(fv));
			}
		}

		private void PostCallback(object state)
		{
			// Make sure we fire ListChanged on the UI thread
			ListChangedEventArgs args = (state as ListChangedEventArgs);
			base.OnListChanged(args);

			// Debug info
			//WriteDebugThreadInfo("PostCallback");
		}

		/*        private static void WriteDebugThreadInfo(string source)
				{
		#if DEBUG
					Thread thread = Thread.CurrentThread;
					string code = thread.GetHashCode().ToString();

					Debug.WriteLine(string.Format("{0}: {1}, background: {2}, ThreadPoolThread: {3}", source, code, thread.IsBackground, thread.IsThreadPoolThread));
		#endif
				}
		 */

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
					MessageBox.Show(ex.Message, AccessDeniedDialogTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
					return;
				}

				_suspend = true;
				try
				{
					Clear();
					_directorySize = 0;

					// Load child files and directories
					foreach (DirectoryInfo di in diArray)
					{
						if ((di.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
							Add(new FileView(di));
					}

					foreach (FileInfo fi in fiArray)
					{
						if ((fi.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
						{
							FileView fv = new FileView(fi);
							_directorySize += fv.Size ?? 0;
							Add(fv);
						}
					}
				}
				finally
				{
					// Resume ListChanged events
					_suspend = false;
				}

				DirectorySizeBytes = _directorySize;

				if (addHistory)
				{
					if (_directory != null)
						_backHistory.Insert(0, _directory);
					_forwardHistory.Clear();
				}

				_directory = new FileView(info);

				// Raise ListChanged event of ListChangedType.Reset
				ResetBindings();

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

				SendEvent(GoBackEnabledChanged);
				SendEvent(GoForwardEnabledChanged);

				// Write debug info
				//WriteDebugThreadInfo("DirectoryView");
			}
			finally
			{
				SendEvent(NavigationComplete);
			}
		}

		private void SendEvent(EventHandler eventHandler)
		{
			EventHandler tmpEventHandler = eventHandler;
			if (tmpEventHandler != null)
				tmpEventHandler(this, EventArgs.Empty);
		}

		/// <summary>
		/// Gets the directory size in bytes.
		/// </summary>
		/// <value>The directory size in bytes.</value>
		public long DirectorySizeBytes
		{
			get { return _directorySize; }
			private set
			{
				_directorySize = value;
				SendEvent(DirectorySizeChanged);
			}
		}

        private static string _accessDeniedDialogTitle = "Access denied";
        /// <summary>
        /// Gets or sets the access denied dialog title.
        /// </summary>
        /// <value>The access denied dialog title.</value>
        public static string AccessDeniedDialogTitle
        {
            get { return _accessDeniedDialogTitle; }
            set { _accessDeniedDialogTitle = value; }
        }
	}
}