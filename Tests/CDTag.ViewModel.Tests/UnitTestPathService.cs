using System;
using System.IO;
using CDTag.Common;
using CDTag.Common.ApplicationServices;

namespace CDTag.ViewModel.Tests
{
    public class UnitTestPathService : IPathService
    {
        private static readonly PathService _pathService = new PathService();
        private string _localApplicationDirectory;
        private string _profileDirectory;

        /// <summary>Gets the local application directory.</summary>
        public string LocalApplicationDirectory
        {
            get
            {
                if (_localApplicationDirectory == null)
                {
                    _localApplicationDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"CD-Tag Unit Tests");
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

        public bool IsShortFileNameValid(string fileName)
        {
            return _pathService.IsShortFileNameValid(fileName);
        }

        public string GetEnglishCharacterReplacement(string input)
        {
            return _pathService.GetEnglishCharacterReplacement(input);
        }
    }
}
