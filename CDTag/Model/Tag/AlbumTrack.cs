using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CDTag.Common;
using IdSharp.Tagging.ID3v1;
using IdSharp.Tagging.ID3v2;

namespace CDTag.Model.Tag
{
    public class AlbumTrack : ModelBase<AlbumTrack>
    {
        private readonly string _path;

        public AlbumTrack(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path");

            _path = path;

            if (ID3v2Tag.DoesTagExist(path))
            {
                ID3v2Tag id3v2 = new ID3v2Tag(path);
                Disc = id3v2.DiscNumber;
                TrackNumber = id3v2.TrackNumber;
                Artist = id3v2.Artist;
                Title = id3v2.Title;
                ReleaseDate = id3v2.OriginalReleaseYear;
            }

            if (ID3v1Tag.DoesTagExist(path))
            {
                ID3v1Tag id3v1 = new ID3v1Tag(path);
                if (string.IsNullOrWhiteSpace(TrackNumber))
                    TrackNumber = string.Format("{0}", id3v1.TrackNumber);
                if (string.IsNullOrWhiteSpace(Artist))
                    Artist = id3v1.Artist;
                if (string.IsNullOrWhiteSpace(Title))
                    Title = id3v1.Title;
                if (string.IsNullOrWhiteSpace(ReleaseDate))
                    ReleaseDate = id3v1.Year;

                // TODO: Genre, album, play length, file size
            }

            // TODO: APE, Lyrics3
        }

        public string OriginalFileName
        {
            get { return _path; }
        }

        public string Disc
        {
            get { return Get<string>("Disc"); }
            set { Set("Disc", value); }
        }

        public string TrackNumber
        {
            get { return Get<string>("TrackNumber"); }
            set { Set("TrackNumber", value); }
        }

        public string Artist
        {
            get { return Get<string>("Artist"); }
            set { Set("Artist", value); }
        }

        public string Title
        {
            get { return Get<string>("Title"); }
            set { Set("Title", value); }
        }

        public string ReleaseDate
        {
            get { return Get<string>("ReleaseDate"); }
            set { Set("ReleaseDate", value); }
        }
    }
}
