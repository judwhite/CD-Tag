﻿using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CDTag.Common.Win32API
{
    /// <summary>
    /// IconHelper
    /// </summary>
    public static class IconHelper
    {
        /// <summary>
        /// Gets the icon as an <see cref="ImageSource"/> for the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="info">The shell file info.</param>
        /// <returns>The icon as an <see cref="ImageSource"/> for the specified path.</returns>
        public static ImageSource GetImageSource(string path, out Win32.SHFILEINFO info)
        {
            info = new Win32.SHFILEINFO();

            int attr;
            if (Directory.Exists(path))
                attr = (int)Win32.FILE_ATTRIBUTE_DIRECTORY;
            else if (File.Exists(path))
                attr = (int)Win32.FILE_ATTRIBUTE_NORMAL;
            else if (path.Length == 3) // drive not ready
                attr = (int)Win32.FILE_ATTRIBUTE_DIRECTORY;
            else
                return null;

            Win32.SHGetFileInfo(path, attr, out info, (uint)Marshal.SizeOf(typeof(Win32.SHFILEINFO)), Win32.SHGFI_DISPLAYNAME | Win32.SHGFI_ICON | Win32.SHGFI_TYPENAME | Win32.SHGFI_SMALLICON | Win32.SHGFI_USEFILEATTRIBUTES);

            ImageSource img = null;
            if (info.hIcon != IntPtr.Zero)
            {
                using (Icon icon = Icon.FromHandle(info.hIcon))
                {
                    img = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, new Int32Rect(0, 0, icon.Width, icon.Height), BitmapSizeOptions.FromEmptyOptions());
                }

                Win32.DestroyIcon(info.hIcon);
            }

            return img;
        }

        /// <summary>Gets the icon as an <see cref="ImageSource" /> for the specified path.</summary>
        /// <param name="path">The path.</param>
        /// <returns>The icon as an <see cref="ImageSource" /> for the specified path.</returns>
        public static ImageSource GetImageSource(string path)
        {
            Win32.SHFILEINFO info;
            return GetImageSource(path, out info);
        }
    }
}
