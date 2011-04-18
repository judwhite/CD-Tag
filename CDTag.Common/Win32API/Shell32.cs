using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CDTag.Common.Win32API
{
    public static partial class Win32
    {
        /// <summary>
        /// SHGetFileInfo
        /// </summary>
        /// <param name="pszPath">The path of the file or directory.</param>
        /// <param name="dwFileAttributes">The file attributes.</param>
        /// <param name="psfi">The SHFILEINFO.</param>
        /// <param name="cbSizeFileInfo">The size file info.</param>
        /// <param name="uFlags">The flags.</param>
        /// <returns></returns>
        [DllImport("shell32.dll", EntryPoint = "SHGetFileInfo", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SHGetFileInfo(String pszPath,
                                             UInt32 dwFileAttributes,
                                             ref SHFILEINFO psfi,
                                             UInt32 cbSizeFileInfo,
                                             UInt32 uFlags);

        /// <summary>
        /// Displays a dialog box that enables the user to select a Shell folder.
        /// </summary>
        /// <param name="lpbi">A pointer to a <see cref="BROWSEINFO"/> structure. Conveys information used to display the dialog box.</param>
        /// <returns>
        /// Returns a pointer to an item identifier list (PIDL) that specifies the location of the selected folder relative to the root of the namespace. 
        /// If the user chooses the Cancel button in the dialog box, the return value is NULL.
        /// It is possible that the PIDL returned is that of a folder shortcut rather than a folder.
        /// It is the responsibility of the calling application to call CoTaskMemFree to free the IDList returned by SHBrowseForFolder when it is no longer needed.
        /// </returns>
        [DllImport("shell32.dll")]
        public static extern IntPtr SHBrowseForFolder(ref BROWSEINFO lpbi);

        /// <summary>
        /// Converts an item identifier list to a file system path. (Note: SHGetPathFromIDList calls the ANSI version, must call SHGetPathFromIDListW for .NET)
        /// </summary>
        /// <param name="pidl">Address of an item identifier list that specifies a file or directory location relative to the root of the namespace (the desktop).</param>
        /// <param name="pszPath">Address of a buffer to receive the file system path. This buffer must be at least <see cref="MAX_PATH"/> characters in size.</param>
        /// <returns><c>true</c> if successful; otherwise, <c>false</c>.</returns>
        [DllImport("shell32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern uint SHGetPathFromIDListW(
            IntPtr pidl,
            [MarshalAs(UnmanagedType.LPTStr)] StringBuilder pszPath
        );
    }
}
