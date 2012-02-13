using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CDTag.Common;
using CDTag.Model.Tag;

namespace CDTag.Model.Profile
{
    public class UserProfile : ModelBase<UserProfile>
    {
        private static readonly IPathService _pathService;

        static UserProfile()
        {
            _pathService = IoC.Resolve<IPathService>();
        }

        public UserProfile()
        {
            FileNaming = new FileNaming();
        }

        public FileNaming FileNaming
        {
            get { return Get<FileNaming>("FileNaming"); }
            set { Set("FileNaming", value); }
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
