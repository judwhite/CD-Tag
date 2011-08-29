using System;
using System.Linq;
using CDTag.Common;
using IdSharp.AudioInfo;
using IdSharp.Tagging.ID3v1;
using IdSharp.Tagging.ID3v2;
using IdSharp.Tagging.ID3v2.Frames;

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
                DiscNumber = id3v2.DiscNumber;
                TrackNumber = id3v2.TrackNumber;
                Artist = id3v2.Artist;
                Title = id3v2.Title;
                ReleaseDate = id3v2.Year;
                Album = id3v2.Album;
                Genre = id3v2.Genre;
                if (id3v2.PictureList != null && id3v2.PictureList.Count == 1)
                {
                    Picture = id3v2.PictureList[0];
                }
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
                if (string.IsNullOrWhiteSpace(Album))
                    Album = id3v1.Album;
                if (string.IsNullOrWhiteSpace(Genre))
                    Genre = GenreHelper.GenreByIndex[id3v1.GenreIndex];
            }

            IAudioFile audioFile = AudioFile.Create(_path, throwExceptionIfUnknown: true);
            Bitrate = audioFile.Bitrate;
            TotalSeconds = audioFile.TotalSeconds;

            // TODO: APE, Lyrics3

            // TODO: When no tags, try to guess from path and file names
            // TODO: Parse Tracks for TotalTracks
            // TODO: Parse Disc for TotalDiscs

            // Parse track # from TrackNumber including total tracks
            if (!string.IsNullOrWhiteSpace(TrackNumber))
            {
                if (TrackNumber.Contains('/'))
                {
                    TrackNumber = TrackNumber.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    // TODO: Set total tracks?
                }
            }

            // Parse disc # from DiscNumber including total discs
            if (!string.IsNullOrWhiteSpace(DiscNumber))
            {
                if (DiscNumber.Contains('/'))
                {
                    DiscNumber = DiscNumber.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    // TODO: Set total discs?
                }
            }
            else
            {
                DiscNumber = "1";
            }
        }

        public IAttachedPicture Picture
        {
            get { return Get<IAttachedPicture>("Picture"); }
            set { Set("Picture", value); }
        }

        public decimal Bitrate
        {
            get { return Get<decimal>("Bitrate"); }
            private set { Set("Bitrate", value); }
        }

        public decimal TotalSeconds
        {
            get { return Get<decimal>("TotalSeconds"); }
            private set { Set("TotalSeconds", value); }
        }

        public string OriginalFileName
        {
            get { return _path; }
        }

        public string DiscNumber
        {
            get { return Get<string>("DiscNumber"); }
            set { Set("DiscNumber", value); }
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

        public string Album
        {
            get { return Get<string>("Album"); }
            set { Set("Album", value); }
        }

        public string Genre
        {
            get { return Get<string>("Genre"); }
            set { Set("Genre", value); }
        }
    }
}
