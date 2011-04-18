using System;

namespace CDTag.Common.Win32API
{
    public static partial class Win32
    {
        /// <summary>FILE_ATTRIBUTE_NORMAL</summary>
        public const UInt32 FILE_ATTRIBUTE_NORMAL = 0x80;
        /// <summary>FILE_ATTRIBUTE_DIRECTORY</summary>
        public const UInt32 FILE_ATTRIBUTE_DIRECTORY = 0x10;

        /// <summary>SHGFI_ICON; Get icon.</summary>
        public const UInt32 SHGFI_ICON = 0x100;
        /// <summary>SHGFI_LARGEICON; Get large icon.</summary>
        public const UInt32 SHGFI_LARGEICON = 0x00;
        /// <summary>SHGFI_SMALLICON; Get small icon.</summary>
        public const UInt32 SHGFI_SMALLICON = 0x01;

        /// <summary>SHGFI_USEFILEATTRIBUTES; Use passed dwFileAttribute.</summary>
        public const UInt32 SHGFI_USEFILEATTRIBUTES = 0x10;
        /// <summary>SHGFI_TYPENAME; Get type name.</summary>
        public const UInt32 SHGFI_TYPENAME = 0x400;

        /// <summary>LOGON32_PROVIDER_DEFAULT</summary>
        public const Int32 LOGON32_PROVIDER_DEFAULT = 0;
        /// <summary>LOGON32_LOGON_INTERACTIVE; This parameter causes LogonUser to create a primary token.</summary>
        public const Int32 LOGON32_LOGON_INTERACTIVE = 2;

        /// <summary>INFINITE</summary>
        public const UInt32 INFINITE = 0xFFFFFFFF;
        /// <summary>WAIT_ABANDONED</summary>
        public const UInt32 WAIT_ABANDONED = 0x80;
        /// <summary>WAIT_OBJECT_0</summary>
        public const UInt32 WAIT_OBJECT_0 = 0x00;
        /// <summary>WAIT_TIMEOUT</summary>
        public const UInt32 WAIT_TIMEOUT = 0x102;
        /// <summary>Closes the source handle. This occurs regardless of any error status returned.</summary>
        public const int DUPLICATE_CLOSE_SOURCE = 1;
        /// <summary>Ignores the dwDesiredAccess parameter. The duplicate handle has the same access as the source handle.</summary>
        public const int DUPLICATE_SAME_ACCESS = 2;
        /// <summary>STARTF_USESHOWWINDOW</summary>
        public const int STARTF_USESHOWWINDOW = 1;
        /// <summary>STARTF_USESTDHANDLES</summary>
        public const int STARTF_USESTDHANDLES = 0x100;
        /// <summary>SW_HIDE</summary>
        public const int SW_HIDE = 0;
        /// <summary>BIF_BROWSEFORCOMPUTER</summary>
        public const uint BIF_BROWSEFORCOMPUTER = 0x1000;
        /// <summary>BIF_BROWSEFORPRINTER</summary>
        public const uint BIF_BROWSEFORPRINTER = 0x2000;
        /// <summary>BIF_BROWSEINCLUDEFILES</summary>
        public const uint BIF_BROWSEINCLUDEFILES = 0x4000;
        /// <summary>BIF_BROWSEINCLUDEURLS</summary>
        public const uint BIF_BROWSEINCLUDEURLS = 0x80;
        /// <summary>BIF_DONTGOBELOWDOMAIN</summary>
        public const uint BIF_DONTGOBELOWDOMAIN = 2;
        /// <summary>BIF_EDITBOX</summary>
        public const uint BIF_EDITBOX = 0x10;
        /// <summary>BIF_NEWDIALOGSTYLE</summary>
        public const uint BIF_NEWDIALOGSTYLE = 0x40;
        /// <summary>BIF_RETURNFSANCESTORS</summary>
        public const uint BIF_RETURNFSANCESTORS = 8;
        /// <summary>BIF_RETURNONLYFSDIRS</summary>
        public const uint BIF_RETURNONLYFSDIRS = 1;
        /// <summary>BIF_SHAREABLE</summary>
        public const uint BIF_SHAREABLE = 0x8000;
        /// <summary>BIF_STATUSTEXT</summary>
        public const uint BIF_STATUSTEXT = 4;
        /// <summary>BIF_USENEWUI</summary>
        public const uint BIF_USENEWUI = 80;
        /// <summary>BIF_VALIDATE</summary>
        public const uint BIF_VALIDATE = 0x20;
        /// <summary>SHGFI_ADDOVERLAYS</summary>
        public const uint SHGFI_ADDOVERLAYS = 0x20;
        /// <summary>SHGFI_ATTR_SPECIFIED</summary>
        public const uint SHGFI_ATTR_SPECIFIED = 0x20000;
        /// <summary>SHGFI_ATTRIBUTES</summary>
        public const uint SHGFI_ATTRIBUTES = 0x800;
        /// <summary>SHGFI_DISPLAYNAME</summary>
        public const uint SHGFI_DISPLAYNAME = 0x200;
        /// <summary>SHGFI_EXETYPE</summary>
        public const uint SHGFI_EXETYPE = 0x2000;
        /// <summary>SHGFI_ICONLOCATION</summary>
        public const uint SHGFI_ICONLOCATION = 0x1000;
        /// <summary>SHGFI_LINKOVERLAY</summary>
        public const uint SHGFI_LINKOVERLAY = 0x8000;
        /// <summary>SHGFI_OPENICON</summary>
        public const uint SHGFI_OPENICON = 2;
        /// <summary>SHGFI_OVERLAYINDEX</summary>
        public const uint SHGFI_OVERLAYINDEX = 0x40;
        /// <summary>SHGFI_PIDL</summary>
        public const uint SHGFI_PIDL = 8;
        /// <summary>SHGFI_SELECTED</summary>
        public const uint SHGFI_SELECTED = 0x10000;
        /// <summary>SHGFI_SHELLICONSIZE</summary>
        public const uint SHGFI_SHELLICONSIZE = 4;
        /// <summary>SHGFI_SYSICONINDEX</summary>
        public const uint SHGFI_SYSICONINDEX = 0x4000;
        /// <summary>MAX_PATH</summary>
        public const int MAX_PATH = 0x100;
    }
}
