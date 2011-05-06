using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CDTag.Common.Win32API;
using CDTag.FileBrowser.ViewModel;

namespace CDTag.FileBrowser.Model
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
        private bool _isSelected;
        private ImageSource _imageSource;

        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Initializes a new instance of the <see cref="FileView"/> class.</summary>
        /// <param name="path">The path.</param>
        public FileView(string path)
            : this(new FileInfo(path))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="FileView"/> class.</summary>
        /// <param name="fileInfo">The file info.</param>
        public FileView(FileSystemInfo fileInfo)
        {
            SetState(fileInfo);
        }

        /// <summary>Gets a value indicating whether this instance represents a directory.</summary>
        /// <value><c>true</c> if this instance represents a directory; otherwise, <c>false</c>.</value>
        public bool IsDirectory
        {
            get { return (Size == null); }
        }

        /// <summary>Gets the name.</summary>
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

        /// <summary>Gets the sort name.</summary>
        /// <value>The sort name.</value>
        public string SortName
        {
            get
            {
                return string.Format("{0}?{1}", IsDirectory ? 0 : 1, Name);
            }
        }

        /// <summary>Gets or sets a value indicating whether this instance is selected.</summary>
        /// <value><c>true</c> if this instance is selected; otherwise, <c>false</c>.</value>
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    SendPropertyChanged("IsSelected");
                }
            }
        }

        /// <summary>Gets the size.</summary>
        /// <value>The size.</value>
        public long? Size
        {
            get { return _size; }
            private set
            {
                if (_size != value)
                {
                    _size = value;
                    SendPropertyChanged("Size");
                }
            }
        }

        /// <summary>Gets the size of the sort.</summary>
        /// <value>The size of the sort.</value>
        public string SortSize
        {
            get
            {
                string value = string.Format("{0}?{1:000000000000}?{2}", IsDirectory ? 0 : 1, Size, Name);
                return value;
            }
        }

        /// <summary>Gets the date modified.</summary>
        /// <value>The date modified.</value>
        public DateTime DateModified
        {
            get { return _dateModified; }
            private set
            {
                if (_dateModified != value)
                {
                    _dateModified = value;
                    SendPropertyChanged("DateModified");
                }
            }
        }

        /// <summary>Gets the sort date modified.</summary>
        public string SortDateModified
        {
            get
            {
                return string.Format("{0}?{1:yyyyMMddhhmmss}?{2}", IsDirectory ? 0 : 1, DateModified, Name);
            }
        }

        /// <summary>Gets the date created.</summary>
        /// <value>The date created.</value>
        public DateTime DateCreated
        {
            get { return _dateCreated; }
            private set
            {
                if (_dateCreated != value)
                {
                    _dateCreated = value;
                    SendPropertyChanged("DateCreated");
                }
            }
        }

        /// <summary>Gets the sort date created.</summary>
        public string SortDateCreated
        {
            get
            {
                return string.Format("{0}?{1:yyyyMMddhhmmss}?{2}", IsDirectory ? 0 : 1, DateCreated, Name);
            }
        }

        /// <summary>Gets the file type.</summary>
        /// <value>The file type.</value>
        public string Type
        {
            get { return _type; }
            private set
            {
                if (_type != value)
                {
                    _type = value;
                    SendPropertyChanged("Type");
                }
            }
        }

        /// <summary>Gets the sort type.</summary>
        /// <value>The sort type.</value>
        public string SortType
        {
            get
            {
                return string.Format("{0}?{1}?{2}", IsDirectory ? 0 : 1, Type, Name);
            }
        }

        /// <summary>Gets the icon.</summary>
        /// <value>The icon.</value>
        public Icon Icon
        {
            get { return _icon; }
            private set
            {
                if (_icon != value)
                {
                    _icon = value;
                    _imageSource = null;
                    SendPropertyChanged("Icon");
                }
            }
        }

        /// <summary>Gets the image source (WPF icon).</summary>
        /// <value>The image source (WPF icon).</value>
        public ImageSource ImageSource
        {
            get
            {
                if (_imageSource == null)
                {
                    Icon icon = Icon;
                    _imageSource = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, new Int32Rect(0, 0, icon.Width, icon.Height), BitmapSizeOptions.FromEmptyOptions());
                }

                return _imageSource;
            }
        }

        /// <summary>Gets the full name.</summary>
        /// <value>The full name.</value>
        public string FullName
        {
            get { return _fullName; }
            private set
            {
                if (_fullName != value)
                {
                    _fullName = value;
                    SendPropertyChanged("FullName");
                }
            }
        }

        /// <summary>Refreshes this instance.</summary>
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

            DriveInfo driveInfo = null;
            if (fileInfo.FullName.Length == 3)
            {
                driveInfo = new DriveInfo(FullName);
            }

            // Check if not a directory (size is not valid)
            if (fileInfo.Exists && fileInfo is FileInfo)
            {
                Size = ((FileInfo)fileInfo).Length;
            }
            else
            {
                Size = null;
            }

            if (driveInfo != null && driveInfo.IsReady)
            {
                try
                {
                    DateModified = fileInfo.LastWriteTime;
                }
                catch
                {
                    // TODO: Log error?
                    DateModified = DateTime.MinValue;
                }

                try
                {
                    DateCreated = fileInfo.CreationTime;
                }
                catch
                {
                    // TODO: Log error?
                    DateCreated = DateTime.MinValue;
                }
            }

            // Get Type Name
            Win32.SHFILEINFO info;// = new Win32.SHFILEINFO();
            Win32.SHGetFileInfo(fileInfo.FullName, 0, out info, (uint)Marshal.SizeOf(typeof(Win32.SHFILEINFO)), Win32.SHGFI_DISPLAYNAME | Win32.SHGFI_ICON | Win32.SHGFI_TYPENAME | Win32.SHGFI_SMALLICON);
            
            if (Size == null)
                Type = "File folder"; // TODO: Localize
            else
                Type = string.Format("{0}", info.szTypeName);

            if (info.hIcon == IntPtr.Zero)
                return;

            // Get ICON
            Icon = Icon.FromHandle(info.hIcon);
        }
    }
}