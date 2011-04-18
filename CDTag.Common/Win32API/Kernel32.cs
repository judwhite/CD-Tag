using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace CDTag.Common.Win32API
{
    public static partial class Win32
    {
        /// <summary>
        /// Creates a new process and its primary thread. The new process runs in the security context of the calling process.
        /// If the calling process is impersonating another user, the new process uses the token for the calling process, not the impersonation token. 
        /// To run the new process in the security context of the user represented by the impersonation token, use the CreateProcessAsUser or CreateProcessWithLogonW function.
        /// </summary>
        /// <param name="lpApplicationName">
        /// The name of the module to be executed. To run a batch file, you must start the command interpreter; set <paramref name="lpApplicationName"/> to cmd.exe and
        /// set <paramref name="lpCommandLine"/> to the following arguments: /c plus the name of the batch file.
        /// </param>
        /// <param name="lpCommandLine">
        /// The command line to be executed. The maximum length of this string is 32,768 characters, including the Unicode terminating null character.
        /// If <paramref name="lpApplicationName"/> is NULL, the module name portion of <paramref name="lpCommandLine"/> is limited to <see cref="MAX_PATH"/> characters.
        /// </param>
        /// <param name="lpProcessAttributes">
        /// A pointer to a <see cref="SECURITY_ATTRIBUTES"/> structure that determines whether the returned handle to the new process object can be inherited by child
        /// processes. If <paramref name="lpProcessAttributes"/> is NULL, the handle cannot be inherited.
        /// </param>
        /// <param name="lpThreadAttributes">
        /// A pointer to a <see cref="SECURITY_ATTRIBUTES"/> structure that determines whether the returned handle to the new thread object can be inherited by child
        /// processes. If <paramref name="lpThreadAttributes"/> is NULL, the handle cannot be inherited. 
        /// </param>
        /// <param name="bInheritHandles">
        /// If this parameter is set to <c>true</c>, each inheritable handle in the calling process is inherited by the new process. If the parameter is <c>false</c>,
        /// the handles are not inherited. Note that inherited handles have the same value and access rights as the original handles.
        /// </param>
        /// <param name="dwCreationFlags">The flags that control the priority class and the creation of the process. For a list of values, see Process Creation Flags.</param>
        /// <param name="lpEnvironment">A pointer to the environment block for the new process. If this parameter is NULL, the new process uses the environment of the calling process.</param>
        /// <param name="lpCurrentDirectory">The full path to the current directory for the process. The string can also specify a UNC path.</param>
        /// <param name="lpStartupInfo">
        /// A pointer to a <see cref="STARTUPINFO"/> or STARTUPINFOEX structure.
        /// To set extended attributes, use a STARTUPINFOEX structure and specify EXTENDED_STARTUPINFO_PRESENT in the dwCreationFlags parameter.
        /// </param>
        /// <param name="lpProcessInformation">
        /// A pointer to a <see cref="PROCESS_INFORMATION"/> structure that receives identification information about the new process.
        /// Handles in <see cref="PROCESS_INFORMATION"/> must be closed with <see cref="CloseHandle"/> when they are no longer needed.
        /// </param>
        /// <returns><c>true</c> if the function succeeds; otherwise, <c>false</c>. To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean CreateProcess(
            String lpApplicationName,
            String lpCommandLine,
            ref SECURITY_ATTRIBUTES lpProcessAttributes,
            ref SECURITY_ATTRIBUTES lpThreadAttributes,
            Boolean bInheritHandles,
            UInt32 dwCreationFlags,
            IntPtr lpEnvironment,
            String lpCurrentDirectory,
            [In] ref STARTUPINFO lpStartupInfo,
            out PROCESS_INFORMATION lpProcessInformation
        );

        /// <summary>
        /// Gets the volume information.
        /// </summary>
        /// <param name="RootPathName">
        /// A pointer to a string that contains the root directory of the volume to be described.
        /// If this parameter is NULL, the root of the current directory is used. A trailing backslash is required. 
        /// For example, you specify \\MyServer\MyShare as "\\MyServer\MyShare\", or the C drive as "C:\".
        /// </param>
        /// <param name="VolumeNameBuffer">A pointer to a buffer that receives the name of a specified volume.</param>
        /// <param name="VolumeNameSize">The length of a volume name buffer, in TCHARs. The maximum buffer size is MAX_PATH+1.</param>
        /// <param name="VolumeSerialNumber">A pointer to a variable that receives the volume serial number.</param>
        /// <param name="MaximumComponentLength">A pointer to a variable that receives the maximum length, in TCHARs, of a file name component that a specified file system supports.</param>
        /// <param name="FileSystemFlags">A pointer to a variable that receives flags associated with the specified file system.</param>
        /// <param name="FileSystemNameBuffer">A pointer to a buffer that receives the name of the file system, for example, the FAT file system or the NTFS file system.</param>
        /// <param name="nFileSystemNameSize">The length of the file system name buffer, in TCHARs. The maximum buffer size is MAX_PATH+1.</param>
        /// <returns>If all the requested information is retrieved, the return value is <c>true</c>. If not all the requested information is retrieved, the return value is <c>false</c>. To get extended error information, call GetLastError.</returns>
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetVolumeInformation(
          string RootPathName,
          StringBuilder VolumeNameBuffer,
          int VolumeNameSize,
          out uint VolumeSerialNumber,
          out uint MaximumComponentLength,
          out uint FileSystemFlags,
          StringBuilder FileSystemNameBuffer,
          int nFileSystemNameSize);
        
        /// <summary>
        /// Closes an open object handle.
        /// </summary>
        /// <param name="hObject">A valid handle to an open object.</param>
        /// <returns><c>true</c> if the function succeeds; otherwise, <c>false</c>. To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.</returns>
        [DllImport("kernel32.DLL", EntryPoint = "CloseHandle", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean CloseHandle(IntPtr hObject);

        /// <summary>
        /// Terminates the specified process and all of its threads.
        /// </summary>
        /// <param name="hProcess">A handle to the process to be terminated. </param>
        /// <param name="uExitCode">The exit code to be used by the process and threads terminated as a result of this call.</param>
        /// <returns><c>true</c> if the function succeeds; otherwise, <c>false</c>. To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean TerminateProcess(IntPtr hProcess, UInt32 uExitCode);

        /// <summary>
        /// Reads data from the specified file or input/output (I/O) device. Reads occur at the position specified by the file pointer if supported by the device.
        /// </summary>
        /// <param name="hFile">
        /// A handle to the device (for example, a file, file stream, physical disk, volume, console buffer, tape drive, socket, communications resource, mailslot, or pipe).
        /// The hFile parameter must have been created with read access. For more information, see Generic Access Rights and File Security and Access Rights.
        /// For asynchronous read operations, hFile can be any handle that is opened with the FILE_FLAG_OVERLAPPED flag by the CreateFile function, or a socket handle returned by the socket or accept function.
        /// </param>
        /// <param name="lpBuffer">
        /// A pointer to the buffer that receives the data read from a file or device.
        /// This buffer must remain valid for the duration of the read operation. The caller must not use this buffer until the read operation is completed.
        /// </param>
        /// <param name="nNumberOfBytesToRead">The maximum number of bytes to be read.</param>
        /// <param name="lpNumberOfBytesRead">
        /// A pointer to the variable that receives the number of bytes read when using a synchronous <paramref name="hFile"/> parameter. <see cref="ReadFile"/>
        /// sets this value to zero before doing any work or error checking. Use NULL for this parameter if this is an asynchronous operation to avoid potentially erroneous results.
        /// This parameter can be NULL only when the <paramref name="lpOverlapped"/> parameter is not NULL.
        /// For more information, see the Remarks section on MSDN.
        /// </param>
        /// <param name="lpOverlapped">A pointer to an OVERLAPPED structure is required if the hFile parameter was opened with FILE_FLAG_OVERLAPPED, otherwise it can be NULL.</param>
        /// <returns><c>true</c> if the function succeeds; otherwise, <c>false</c>. To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean ReadFile(
            IntPtr hFile, 
            Byte[] lpBuffer,
            UInt32 nNumberOfBytesToRead, 
            out UInt32 lpNumberOfBytesRead, 
            IntPtr lpOverlapped
        );

        /// <summary>
        /// Waits until the specified object is in the signaled state or the time-out interval elapses.
        /// To enter an alertable wait state, use the WaitForSingleObjectEx function. To wait for multiple objects, use WaitForMultipleObjects.
        /// </summary>
        /// <param name="handle">A handle to the object.</param>
        /// <param name="milliseconds">
        /// The time-out interval, in milliseconds. If a nonzero value is specified, the function waits until the object is signaled or the 
        /// interval elapses. If dwMilliseconds is zero, the function does not enter a wait state if the object is not signaled; it always
        /// returns immediately. If dwMilliseconds is <see cref="INFINITE"/>, the function will return only when the object is signaled.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value indicates the event that caused the function to return. It can be one of the following values:
        /// WAIT_ABANDONED
        /// WAIT_OBJECT_0
        /// WAIT_TIMEOUT
        /// WAIT_FAILED
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern Int32 WaitForSingleObject(IntPtr handle, Int32 milliseconds);

        /// <summary>
        /// Copies data from a named or anonymous pipe into a buffer without removing it from the pipe. It also returns information about data in the pipe.
        /// </summary>
        /// <param name="hNamedPipe">A handle to the pipe.</param>
        /// <param name="lpBuffer">A pointer to a buffer that receives data read from the pipe. This parameter can be NULL if no data is to be read.</param>
        /// <param name="nBufferSize">The size of the buffer specified by the <paramref name="lpBuffer"/> parameter, in bytes. This parameter is ignored if lpBuffer is NULL.</param>
        /// <param name="lpBytesRead">A pointer to a variable that receives the number of bytes read from the pipe. This parameter can be NULL if no data is to be read.</param>
        /// <param name="lpTotalBytesAvail">A pointer to a variable that receives the total number of bytes available to be read from the pipe. This parameter can be NULL if no data is to be read.</param>
        /// <param name="lpBytesLeftThisMessage">
        /// A pointer to a variable that receives the number of bytes remaining in this message.
        /// This parameter will be zero for byte-type named pipes or for anonymous pipes. This parameter can be NULL if no data is to be read.
        /// </param>
        /// <returns><c>true</c> if the function succeeds; otherwise, <c>false</c>. To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.</returns>
        [DllImport("kernel32.dll", EntryPoint = "PeekNamedPipe", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean PeekNamedPipe(
            IntPtr hNamedPipe,
            Byte[] lpBuffer, 
            UInt32 nBufferSize,
            ref UInt32 lpBytesRead,
            ref UInt32 lpTotalBytesAvail,
            ref UInt32 lpBytesLeftThisMessage
        );

        /// <summary>
        /// Creates an anonymous pipe, and returns handles to the read and write ends of the pipe.
        /// </summary>
        /// <param name="hReadPipe">A pointer to a variable that receives the read handle for the pipe.</param>
        /// <param name="hWritePipe">A pointer to a variable that receives the write handle for the pipe.</param>
        /// <param name="lpPipeAttributes">
        /// A pointer to a <see cref="SECURITY_ATTRIBUTES"/> structure that determines whether the returned handle can be inherited by child processes.
        /// If <paramref name="lpPipeAttributes"/> is NULL, the handle cannot be inherited. 
        /// </param>
        /// <param name="nSize">
        /// The size of the buffer for the pipe, in bytes. The size is only a suggestion; the system uses the value to calculate an appropriate buffering mechanism.
        /// If this parameter is zero, the system uses the default buffer size.
        /// </param>
        /// <returns><c>true</c> if the function succeeds; otherwise, <c>false</c>. To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean CreatePipe(
            out IntPtr hReadPipe, 
            out IntPtr hWritePipe,
            ref SECURITY_ATTRIBUTES lpPipeAttributes, 
            UInt32 nSize
        );

        /// <summary>
        /// Retrieves a pseudo handle for the current process.
        /// </summary>
        /// <returns>A pseudo handle to the current process.</returns>
        [DllImport("kernel32.dll")]
        public static extern SafeWaitHandle GetCurrentProcess();

        /// <summary>
        /// Duplicates an object handle.
        /// </summary>
        /// <param name="hSourceProcessHandle">A handle to the process with the handle to be duplicated.</param>
        /// <param name="hSourceHandle">The handle to be duplicated.</param>
        /// <param name="hTargetProcessHandle">A handle to the process that is to receive the duplicated handle.</param>
        /// <param name="lpTargetHandle">A pointer to a variable that receives the duplicate handle. This handle value is valid in the context of the target process.</param>
        /// <param name="dwDesiredAccess">
        /// The access requested for the new handle.
        /// This parameter is ignored if the <paramref name="dwOptions"/> parameter specifies the <see cref="DUPLICATE_SAME_ACCESS"/> flag.
        /// Otherwise, the flags that can be specified depend on the type of object whose handle is to be duplicated.
        /// </param>
        /// <param name="bInheritHandle">
        /// A variable that indicates whether the handle is inheritable.
        /// If <c>true</c>, the duplicate handle can be inherited by new processes created by the target process.
        /// If <c>false</c>, the new handle cannot be inherited.</param>
        /// <param name="dwOptions">
        /// Optional actions. This parameter can be zero, or any combination of the following values:
        /// DUPLICATE_CLOSE_SOURCE: Closes the source handle. This occurs regardless of any error status returned.
        /// DUPLICATE_SAME_ACCESS: Ignores the dwDesiredAccess parameter. The duplicate handle has the same access as the source handle.
        /// </param>
        /// <returns>
        /// 	<c>true</c> if the function succeeds; otherwise, <c>false</c>. To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean DuplicateHandle(
            IntPtr hSourceProcessHandle,
            IntPtr hSourceHandle, 
            IntPtr hTargetProcessHandle, 
            out IntPtr lpTargetHandle,
            UInt32 dwDesiredAccess, 
            Boolean bInheritHandle,
            UInt32 dwOptions
        );

        /// <summary>
        /// Retrieves the termination status of the specified process.
        /// </summary>
        /// <param name="hProcess">A handle to the process.</param>
        /// <param name="lpExitCode">
        /// A pointer to a variable to receive the process termination status.
        /// If the process has not terminated and the function succeeds, the status returned is STILL_ACTIVE.
        /// </param>
        /// <returns>
        /// 	<c>true</c> if the function succeeds; otherwise, <c>false</c>. To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.
        /// </returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean GetExitCodeProcess(IntPtr hProcess, out UInt32 lpExitCode);
    }
}
