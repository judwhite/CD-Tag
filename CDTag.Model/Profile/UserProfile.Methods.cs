using System;
using System.IO;
using CDTag.Common;
using CDTag.Common.Json;
using CDTag.Model.Tag;

namespace CDTag.Model.Profile
{
    public partial class UserProfile : ModelBase<UserProfile>
    {
        public void Save()
        {
            string message;
            if (!ValidateProfileName(out message))
                throw new Exception(message);

            string path = GetProfileFileName();
            string json = JsonSerializer.SerializeObject(this);
            File.WriteAllText(path, json);
        }

        public bool ValidateProfileName(out string message)
        {
            // No profile name entered
            if (string.IsNullOrWhiteSpace(ProfileName))
            {
                message = "Please enter a name for your profile."; // TODO: Localize
                return false;
            }

            // Invalid characters
            if (!_pathService.IsShortFileNameValid(ProfileName))
            {
                message = string.Format("'{0}' contains invalid characters.", ProfileName); // TODO: Localize
                return false;
            }

            message = null;
            return true;
        }

        public string GetProfileFileName()
        {
            string fileName = Path.Combine(_pathService.ProfileDirectory, ProfileName);

            string ext = Path.GetExtension(fileName);
            if (string.Compare(ext, ".cfg", ignoreCase: true) != 0)
                fileName += ".cfg";

            return fileName;
        }

        public string GetFileName(string format, AlbumTrack track, string extension = null)
        {
            if (string.IsNullOrWhiteSpace(format))
                throw new ArgumentNullException("format");
            if (track == null)
                throw new ArgumentNullException("track");

            format = format.Replace("<Year>", track.ReleaseDate);
            format = format.Replace("<Artist>", track.Artist);
            format = format.Replace("<Album>", track.Album);
            format = format.Replace("<Title>", track.Title);
            format = format.Replace("<Song>", track.Title);

            int trackNumber;
            int.TryParse(track.TrackNumber, out trackNumber);
            format = format.Replace("<Track>", string.Format("{0:00}", trackNumber));

            format = ApplyFormattingRules(format);

            if (extension != null)
            {
                format += extension;
            }

            return format;
        }

        public string GetDirectoryName(string format, Album album)
        {
            if (string.IsNullOrWhiteSpace(format))
                throw new ArgumentNullException("format");
            if (album == null)
                throw new ArgumentNullException("album");

            format = format.Replace("<Year>", album.ReleaseDate);
            format = format.Replace("<Artist>", album.Artist);
            format = format.Replace("<Album>", album.AlbumTitle);

            format = ApplyFormattingRules(format);

            return format;
        }

        private string ApplyFormattingRules(string format)
        {
            if (string.IsNullOrWhiteSpace(format))
                throw new ArgumentNullException("format");

            if (FileNaming.UseUnderscores)
            {
                format = format.Replace(" ", "_");
            }

            if (FileNaming.UseLatinCharactersOnly || FileNaming.UseStandardCharactersOnly)
            {
                format = _pathService.GetEnglishCharacterReplacement(format);
            }

            // TODO: Find/Replace

            if (FileNaming.UseStandardCharactersOnly)
            {
                for (int i = 0; i < format.Length; i++)
                {
                    char c = format[i];
                    if (c == '-' || c == '_' || c == '.' || c == '(' || c == ')' || (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == ' ' || (c >= '0' && c <= '9'))
                        continue;

                    format = format.Replace(c, ' ');
                }
            }

            if (FileNaming.UseUnderscores)
            {
                format = format.Replace(" ", "_");
            }

            // TODO: Look for word separators at end
            format = format.TrimEnd(new[] { ' ', '_' });

            return format;
        }
    }
}
