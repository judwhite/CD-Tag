using System;
using System.Runtime.InteropServices;

namespace CDTag.Common.Win32API
{
    public static partial class Win32
    {
        /// <summary>
        /// Destroys an icon.
        /// </summary>
        /// <param name="hIcon">The icon handle.</param>
        [DllImport("user32.dll")]
        public static extern Int32 DestroyIcon(IntPtr hIcon);
    }
}
