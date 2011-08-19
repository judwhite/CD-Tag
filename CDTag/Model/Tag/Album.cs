using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CDTag.Common;

namespace CDTag.Model.Tag
{
    public class Album : ModelBase<Album>
    {
        private readonly string _path;

        public Album(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path");

            _path = path;

            string[] files = Directory.GetFiles(_path, "*.mp3");
            if (files == null || files.Length == 0)
                throw new Exception("No audio files found"); // TODO: Localize ?
        }

        public string OldDirectoryName
        {
            get { return _path; }
        }

        public string Artist
        {
            get { return Get<string>("Artist"); }
            set { Set("Artist", value); }
        }

        public string AlbumTitle
        {
            get { return Get<string>("AlbumTitle"); }
            set { Set("AlbumTitle", value); }
        }

        public string Genre
        {
            get { return Get<string>("Genre"); }
            set { Set("Genre", value); }
        }

        public string ReleaseDate
        {
            get { return Get<string>("ReleaseDate"); }
            set { Set("ReleaseDate", value); }
        }
    }
}
