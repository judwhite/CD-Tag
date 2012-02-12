using System;
using System.Collections.Generic;
using System.IO;

namespace CDTag.Common
{
    /// <summary>
    /// PathService
    /// </summary>
    public class PathService : IPathService
    {
        private string _localApplicationDirectory;
        private string _profileDirectory;

        /// <summary>Gets the local application directory.</summary>
        public string LocalApplicationDirectory
        {
            get
            {
                if (_localApplicationDirectory == null)
                {
                    _localApplicationDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"CD-Tag");
                    if (!Directory.Exists(_localApplicationDirectory))
                        Directory.CreateDirectory(_localApplicationDirectory);
                }

                return _localApplicationDirectory;
            }
        }

        /// <summary>Gets the profile directory.</summary>
        public string ProfileDirectory
        {
            get
            {
                if (_profileDirectory == null)
                {
                    _profileDirectory = Path.Combine(LocalApplicationDirectory, "Profiles");
                    if (!Directory.Exists(_profileDirectory))
                        Directory.CreateDirectory(_profileDirectory);
                }

                return _profileDirectory;
            }
        }

        /// <summary>Determines whether a short file name is valid.</summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>
        ///   <c>true</c> if the short file name is valid; otherwise, <c>false</c>.
        /// </returns>
        public bool IsShortFileNameValid(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            string trimmedFileName = fileName.Trim();
            if (trimmedFileName == "." || trimmedFileName == "..")
                return false;

            var invalidChars = new List<char>();
            invalidChars.Add('/');
            invalidChars.Add('\\');
            invalidChars.AddRange(Path.GetInvalidFileNameChars());
            invalidChars.AddRange(Path.GetInvalidPathChars());

            foreach (char c in fileName)
            {
                if (invalidChars.Contains(c))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
