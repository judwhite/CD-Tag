using System;
using System.Runtime.InteropServices;

namespace CDTag.Common.Win32API
{
    public static partial class Win32
    {
        /// <summary>
        /// Frees a block of task memory previously allocated through a call to the CoTaskMemAlloc or CoTaskMemRealloc function.
        /// </summary>
        /// <param name="pv">A pointer to the memory block to be freed.</param>
        [DllImport("ole32.dll")]
        public static extern void CoTaskMemFree(IntPtr pv);
    }
}
