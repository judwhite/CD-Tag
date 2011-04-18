using System;
using System.Runtime.InteropServices;

namespace CDTag.Common.Win32API
{
    public static partial class Win32
    {
        /// <summary>
        /// Specifies an application-defined callback function used to send messages to, and process messages from, a Browse dialog box displayed in response to a call to <see cref="SHBrowseForFolder"/>.
        /// </summary>
        /// <param name="hwnd">The window handle of the browse dialog box.</param>
        /// <param name="uMsg">
        /// The dialog box event that generated the message. One of the following values:
        /// BFFM_INITIALIZED: The dialog box has finished initializing.
        /// BFFM_IUNKNOWN: An IUnknown interface is available to the dialog box.
        /// BFFM_SELCHANGED: The selection has changed in the dialog box.
        /// BFFM_VALIDATEFAILED: Version 4.71. The user typed an invalid name into the dialog's edit box. A nonexistent folder is considered an invalid name.
        /// </param>
        /// <param name="lParam">
        /// A value whose meaning depends on the event specified in uMsg as follows:
        /// BFFM_INITIALIZED: Not used, value is NULL.
        /// BFFM_IUNKNOWN: A pointer to an IUnknown interface.
        /// BFFM_SELCHANGED: A pointer to an item identifier list (PIDL) identifying the newly selected item.
        /// BFFM_VALIDATEFAILED: A pointer to a string containing the invalid name. An application can use this data in an error dialog informing the user that the name was not valid.
        /// </param>
        /// <param name="lpData">An application-defined value that was specified in the lParam member of the BROWSEINFO structure used in the call to SHBrowseForFolder.</param>
        /// <returns>
        /// This function always returns zero.
        /// </returns>
        public delegate int BrowseCallbackProc(IntPtr hwnd, int uMsg, IntPtr lParam, IntPtr lpData);

        /// <summary>Contains information about a file object. Used by <see cref="SHGetFileInfo"/>.</summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHFILEINFO
        {
            /// <summary>Handle to the icon that represents the file.</summary>
            public IntPtr hIcon;
            /// <summary>Index of the icon image within the system image list.</summary>
            public IntPtr iIcon;
            /// <summary>Array of values that indicates the attributes of the file object.</summary>
            public UInt32 dwAttributes;
            /// <summary>String that contains the name of the file as it appears in the Microsoft Windows Shell, or the path and file name of the 
            /// file that contains the icon representing the file.</summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public String szDisplayName;
            /// <summary>String that describes the type of file.</summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public String szTypeName;
        }

        /// <summary>
        /// Used with <see cref="CreateProcess"/>, CreateProcessAsUser, and CreateProcessWithLogonW to specify the window station, desktop, standard handles, 
        /// and appearance of the main window for the new process.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct STARTUPINFO
        {
            /// <summary>The size of the structure, in bytes.</summary>
            public Int32 cb;
            /// <summary>Reserved; must be NULL.</summary>
            public String lpReserved;
            /// <summary>The name of the desktop, or the name of both the desktop and window station for this process. A backslash in the string indicates 
            /// that the string includes both the desktop and window station names. For more information, see Thread Connection to a Desktop at 
            /// http://msdn.microsoft.com/en-us/library/ms686744(VS.85).aspx.</summary>
            public String lpDesktop;
            /// <summary>For console processes, this is the title displayed in the title bar if a new console window is created. If NULL, the name of the
            /// executable file is used as the window title instead. This parameter must be NULL for GUI or console processes that do not create a new console window.</summary>
            public String lpTitle;
            /// <summary>
            /// If <see cref="dwFlags"/> specifies STARTF_USEPOSITION, this member is the x offset of the upper left corner of a window if a new window is created, in pixels. Otherwise, this member is ignored.
            /// The offset is from the upper left corner of the screen. For GUI processes, the specified position is used the first time the new process calls CreateWindow to create an overlapped window if the x parameter of CreateWindow is CW_USEDEFAULT.
            /// </summary>
            public Int32 dwX;
            /// <summary>
            /// If <see cref="dwFlags"/> specifies STARTF_USEPOSITION, this member is the y offset of the upper left corner of a window if a new window is created, in pixels. Otherwise, this member is ignored.
            /// The offset is from the upper left corner of the screen. For GUI processes, the specified position is used the first time the new process calls CreateWindow to create an overlapped window if the y parameter of CreateWindow is CW_USEDEFAULT.
            /// </summary>
            public Int32 dwY;
            /// <summary>
            /// If <see cref="dwFlags"/> specifies STARTF_USESIZE, this member is the width of the window if a new window is created, in pixels. Otherwise, this member is ignored.
            /// For GUI processes, this is used only the first time the new process calls CreateWindow to create an overlapped window if the nWidth parameter of CreateWindow is CW_USEDEFAULT.
            /// </summary>
            public Int32 dwXSize;
            /// <summary>
            /// If <see cref="dwFlags"/> specifies STARTF_USESIZE, this member is the height of the window if a new window is created, in pixels. Otherwise, this member is ignored.
            /// For GUI processes, this is used only the first time the new process calls CreateWindow to create an overlapped window if the nHeight parameter of CreateWindow is CW_USEDEFAULT.
            /// </summary>
            public Int32 dwYSize;
            /// <summary>
            /// If <see cref="dwFlags"/> specifies STARTF_USECOUNTCHARS, if a new console window is created in a console process, this member specifies the screen buffer width, in character columns. Otherwise, this member is ignored.
            /// </summary>
            public Int32 dwXCountChars;
            /// <summary>
            /// If <see cref="dwFlags"/> specifies STARTF_USECOUNTCHARS, if a new console window is created in a console process, this member specifies the screen buffer height, in character rows. Otherwise, this member is ignored.
            /// </summary>
            public Int32 dwYCountChars;
            /// <summary>
            /// If <see cref="dwFlags"/> specifies STARTF_USEFILLATTRIBUTE, this member is the initial text and background colors if a new console window is created in a console application. Otherwise, this member is ignored.
            /// This value can be any combination of the following values: FOREGROUND_BLUE, FOREGROUND_GREEN, FOREGROUND_RED, FOREGROUND_INTENSITY, BACKGROUND_BLUE, BACKGROUND_GREEN, BACKGROUND_RED, and BACKGROUND_INTENSITY.
            /// For example, the following combination of values produces red text on a white background:
            /// FOREGROUND_RED| BACKGROUND_RED| BACKGROUND_GREEN| BACKGROUND_BLUE
            /// </summary>
            public Int32 dwFillAttribute;
            /// <summary>
            /// A bit field that determines whether certain STARTUPINFO members are used when the process creates a window. This member can be one or more of the following values.
            /// STARTF_FORCEONFEEDBACK
            /// STARTF_FORCEOFFFEEDBACK
            /// STARTF_RUNFULLSCREEN
            /// STARTF_USECOUNTCHARS
            /// STARTF_USEFILLATTRIBUTE
            /// STARTF_USEPOSITION
            /// STARTF_USESHOWWINDOW
            /// STARTF_USESIZE
            /// STARTF_USESTDHANDLES
            /// </summary>
            public Int32 dwFlags;
            /// <summary>
            /// If <see cref="dwFlags"/> specifies STARTF_USESHOWWINDOW, this member can be any of the SW_ constants defined in Winuser.h. Otherwise, this member is ignored.
            /// For GUI processes, wShowWindow specifies the default value the first time ShowWindow is called. The nCmdShow parameter of ShowWindow is ignored. In subsequent calls to ShowWindow, the wShowWindow member is used if the nCmdShow parameter of ShowWindow is set to SW_SHOWDEFAULT.
            /// </summary>
            public Int16 wShowWindow;
            /// <summary>
            /// Reserved for use by the C Run-time; must be zero.
            /// </summary>
            public Int16 cbReserved2;
            /// <summary>
            /// Reserved for use by the C Run-time; must be NULL.
            /// </summary>
            public IntPtr lpReserved2;
            /// <summary>
            /// If <see cref="dwFlags"/> specifies STARTF_USESTDHANDLES, this member is the standard input handle for the process. Otherwise, this member is ignored and the default for standard input is the keyboard buffer.
            /// </summary>
            public IntPtr hStdInput;
            /// <summary>
            /// If <see cref="dwFlags"/> specifies STARTF_USESTDHANDLES, this member is the standard output handle for the process. Otherwise, this member is ignored and the default for standard output is the console window's buffer.
            /// </summary>
            public IntPtr hStdOutput;
            /// <summary>
            /// If <see cref="dwFlags"/> specifies STARTF_USESTDHANDLES, this member is the standard error handle for the process. Otherwise, this member is ignored and the default for standard error is the console window's buffer.
            /// </summary>
            public IntPtr hStdError;
        }

        /// <summary>
        /// The PROCESS_INFORMATION structure is filled in by either the CreateProcess, CreateProcessAsUser, CreateProcessWithLogonW, or CreateProcessWithTokenW function with information about the newly created process and its primary thread.
        /// </summary>
        /// <remarks>
        /// If the function succeeds, be sure to call the <see cref="CloseHandle"/> function to close the hProcess and hThread handles when you are finished with them. Otherwise, when the child 
        /// process exits, the system cannot clean up the process structures for the child process because the parent process still has open handles to the child process. However, the system 
        /// will close these handles when the parent process terminates, so the structures related to the child process object would be cleaned up at this point.
        /// </remarks>
        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            /// <summary>
            /// A handle to the newly created process. The handle is used to specify the process in all functions that perform operations on the process object.
            /// </summary>
            public IntPtr hProcess;
            /// <summary>
            /// A handle to the primary thread of the newly created process. The handle is used to specify the thread in all functions that perform operations on the thread object.
            /// </summary>
            public IntPtr hThread;
            /// <summary>
            /// A value that can be used to identify a process. The value is valid from the time the process is created until all handles to the process are closed and the process object is freed; at this point, the identifier may be reused.
            /// </summary>
            public Int32 dwProcessId;
            /// <summary>
            /// A value that can be used to identify a thread. The value is valid from the time the thread is created until all handles to the thread are closed and the thread object is freed; at this point, the identifier may be reused.
            /// </summary>
            public Int32 dwThreadId;
        }

        /// <summary>
        /// The SECURITY_ATTRIBUTES structure contains the security descriptor for an object and specifies whether the handle retrieved by specifying this structure is inheritable. This structure provides security settings for objects created by various functions, such as CreateFile, CreatePipe, CreateProcess, RegCreateKeyEx, or RegSaveKeyEx.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            /// <summary>
            /// The size, in bytes, of this structure. Set this value to the size of the SECURITY_ATTRIBUTES structure.
            /// </summary>
            public Int32 nLength;
            /// <summary>
            /// A pointer to a security descriptor for the object that controls the sharing of it. If NULL is specified for this member,
            /// the object is assigned the default security descriptor of the calling process. This is not the same as granting access to
            /// everyone by assigning a NULL discretionary access control list (DACL). The default security descriptor is based on the
            /// default DACL of the access token belonging to the calling process. By default, the default DACL in the access token of a
            /// process allows access only to the user represented by the access token. If other users must access the object, you can
            /// either create a security descriptor with the appropriate access, or add ACEs to the DACL that grants access to a group
            /// of users.
            /// </summary>
            public IntPtr lpSecurityDescriptor;
            /// <summary>
            /// A Boolean value that specifies whether the returned handle is inherited when a new process is created. If this member is
            /// TRUE, the new process inherits the handle.
            /// </summary>
            public Int32 bInheritHandle;
        }

        /// <summary>
        /// Contains parameters for the <see cref="SHBrowseForFolder"/> function and receives information about the folder selected by the user.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct BROWSEINFO
        {
            /// <summary>
            /// A handle to the owner window for the dialog box.
            /// </summary>
            public IntPtr hwndOwner;
            /// <summary>
            /// A pointer to an item identifier list (PIDL) that specifies the location of the root folder from which to start browsing. Only 
            /// the specified folder and its subfolders in the namespace hierarchy appear in the dialog box. This member can be NULL; in that 
            /// case, the namespace root (the Desktop folder) is used.
            /// </summary>
            public IntPtr pidlRoot;
            /// <summary>
            /// Pointer to a buffer to receive the display name of the folder selected by the user. The size of this buffer is assumed to be MAX_PATH characters.
            /// </summary>
            [MarshalAs(UnmanagedType.LPTStr)]
            public string pszDisplayName;
            /// <summary>
            /// Pointer to a null-terminated string that is displayed above the tree view control in the dialog box. This string can be used to specify instructions to the user.
            /// </summary>
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpszTitle;
            /// <summary>
            /// Flags that specify the options for the dialog box. This member can be 0 or a combination of the following values. Version numbers refer to the minimum version of Shell32.dll required for <see cref="SHBrowseForFolder"/> to recognize flags added in later releases. See Shell and Common Controls Versions for more information.
            /// </summary>
            public uint ulFlags;
            /// <summary>
            /// Pointer to an application-defined function that the dialog box calls when an event occurs. For more information, see the 
            /// <see cref="BrowseCallbackProc"/> function. This member can be NULL.
            /// </summary>
            public BrowseCallbackProc lpfn;
            /// <summary>
            /// An application-defined value that the dialog box passes to the callback function, if one is specified in <see cref="lpfn"/>.
            /// </summary>
            public IntPtr lParam;
            /// <summary>
            /// An integer value that receives the index of the image associated with the selected folder, stored in the system image list.
            /// </summary>
            public int iImage;
        }
    }
}
