using System;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CDTag.Common.Win32API;

namespace CDTag.FileBrowser
{
    /// <summary>
    /// FileView class. Represents a file or directory in the file system. See <see cref="DirectoryController"/>.
    /// </summary>
    public class FileView : INotifyPropertyChanged
    {
        private string _name;
        private long? _size;
        private DateTime _dateModified;
        private DateTime _dateCreated;
        private string _type;
        private Icon _icon;
        private string _fullName;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileView"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public FileView(string path)
            : this(new FileInfo(path))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileView"/> class.
        /// </summary>
        /// <param name="fileInfo">The file info.</param>
        public FileView(FileSystemInfo fileInfo)
        {
            SetState(fileInfo);
        }

        /// <summary>
        /// Gets a value indicating whether this instance represents a directory.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance represents a directory; otherwise, <c>false</c>.
        /// </value>
        public bool IsDirectory
        {
            get { return (Size == null); }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _name; }
            private set
            {
                _name = value;
                SendPropertyChanged("Name");
            }
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        public long? Size
        {
            get { return _size; }
            private set
            {
                _size = value;
                SendPropertyChanged("Size");
            }
        }

        /// <summary>
        /// Gets the date modified.
        /// </summary>
        /// <value>The date modified.</value>
        public DateTime DateModified
        {
            get { return _dateModified; }
            private set
            {
                _dateModified = value;
                SendPropertyChanged("DateModified");
            }
        }

        /// <summary>
        /// Gets the date created.
        /// </summary>
        /// <value>The date created.</value>
        public DateTime DateCreated
        {
            get { return _dateCreated; }
            private set
            {
                _dateCreated = value;
                SendPropertyChanged("DateCreated");
            }
        }

        /// <summary>
        /// Gets the file type.
        /// </summary>
        /// <value>The file type.</value>
        public string Type
        {
            get { return _type; }
            private set
            {
                _type = value;
                SendPropertyChanged("Type");
            }
        }

        /// <summary>
        /// Gets the icon.
        /// </summary>
        /// <value>The icon.</value>
        public Icon Icon
        {
            get { return _icon; }
            private set
            {
                _icon = value;
                SendPropertyChanged("Icon");
            }
        }

        /// <summary>
        /// Gets the image source (WPF icon).
        /// </summary>
        /// <value>The image source (WPF icon).</value>
        public ImageSource ImageSource
        {
            get
            {
                Icon icon = Icon;
                return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, new Int32Rect(0, 0, icon.Width, icon.Height), BitmapSizeOptions.FromEmptyOptions());
            }
        }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName
        {
            get { return _fullName; }
            private set
            {
                _fullName = value;
                SendPropertyChanged("FullName");
            }
        }

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        public void Refresh()
        {
            SetState(new FileInfo(FullName));
        }

        internal void Refresh(FileSystemInfo fileSystemInfo)
        {
            // Reset state - name changed
            SetState(fileSystemInfo);
        }

        private void SendPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetState(FileSystemInfo fileInfo)
        {
            FullName = fileInfo.FullName;
            Name = fileInfo.Name;

            // Check if not a directory (size is not valid)
            if (fileInfo.Exists && fileInfo is FileInfo)
            {
                Size = ((FileInfo)fileInfo).Length;
            }
            else
            {
                Size = null;
            }

            DateModified = fileInfo.LastWriteTime;
            DateCreated = fileInfo.CreationTime;

            // Get Type Name
            Win32.SHFILEINFO info = new Win32.SHFILEINFO();
            Win32.SHGetFileInfo(fileInfo.FullName, 0, ref info, (uint)Marshal.SizeOf(info), Win32.SHGFI_ICON | Win32.SHGFI_TYPENAME | Win32.SHGFI_SMALLICON);

            Type = string.Format("{0}", info.szTypeName);

            if (info.hIcon == IntPtr.Zero)
                return;

            // Get ICON
            Icon = Icon.FromHandle(info.hIcon);
        }
    }
}