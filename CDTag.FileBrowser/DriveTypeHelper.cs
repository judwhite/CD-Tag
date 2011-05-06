using System;
using System.IO;

namespace CDTag.FileBrowser
{
    /// <summary>
    /// DriveTypeHelper
    /// </summary>
    public static class DriveTypeHelper
    {
        // TODO: Get string values from a resource file or make them public
        private const string DriveTypes_LocalDisk = "Local Disk";
        private const string DriveTypes_CDDVD = "CD/DVD ({0})";
        private const string DriveTypes_RemovableDisk = "Removable Disk ({0})";

        /// <summary>Gets the drive description.</summary>
        /// <param name="drive">The drive.</param>
        /// <returns>The drive description.</returns>
        public static string GetDescription(DriveInfo drive)
        {
            if (drive == null)
                throw new ArgumentNullException("drive");

            string text = null;

            string volumeLabel = string.Empty;
            if (drive.IsReady)
                volumeLabel = drive.VolumeLabel;
            string driveLetter = drive.Name.TrimEnd('\\');

            switch (drive.DriveType)
            {
                case DriveType.Fixed:
                    if (string.IsNullOrEmpty(volumeLabel))
                        volumeLabel = DriveTypes_LocalDisk;
                    break;
                case DriveType.CDRom:
                    text = string.Format(DriveTypes_CDDVD, driveLetter);
                    break;
                case DriveType.Removable:
                    text = string.Format(DriveTypes_RemovableDisk, driveLetter);
                    break;
                default:
                    text = string.Format("{0}", driveLetter);
                    break;
            }

            if (drive.DriveType == DriveType.Fixed)
            {
                text = string.Format("{0} ({1})", volumeLabel, driveLetter);
            }
            else if (!string.IsNullOrEmpty(volumeLabel))
            {
                text += " " + volumeLabel;
            }

            return text;
        }
    }
}
