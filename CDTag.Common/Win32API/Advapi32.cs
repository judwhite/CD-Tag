using System;
using System.Runtime.InteropServices;

namespace CDTag.Common.Win32API
{
    /// <summary>
    /// Win32 API.
    /// </summary>
    public static partial class Win32
    {
        /// <summary>
        /// Logon as the specified user.
        /// </summary>
        /// <param name="lpszUsername">The username.</param>
        /// <param name="lpszDomain">The domain.</param>
        /// <param name="lpszPassword">The password.</param>
        /// <param name="dwLogonType">Type logon type.</param>
        /// <param name="dwLogonProvider">The logon provider.</param>
        /// <param name="phToken">The token.</param>
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern Boolean LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
                                                Int32 dwLogonType, Int32 dwLogonProvider, ref IntPtr phToken);

        /// <summary>
        /// Duplicates the token.
        /// </summary>
        /// <param name="ExistingTokenHandle">The existing token handle.</param>
        /// <param name="SECURITY_IMPERSONATION_LEVEL">The security impersonation level.</param>
        /// <param name="DuplicateTokenHandle">The duplicate token handle.</param>
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static Boolean DuplicateToken(IntPtr ExistingTokenHandle,
                                                     Int32 SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);
    }
}
