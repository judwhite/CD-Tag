﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;
using CDTag.Common;
using IdSharp.Tagging.ID3v2;

namespace CDTag.Model.Tag
{
    public class Album : ModelBase<Album>
    {
        private readonly string _path;
        private readonly ObservableCollection<AlbumTrack> _tracks;

        public Album(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("path");

            _path = path;

            string[] files = Directory.GetFiles(_path, "*.mp3");
            if (files == null || files.Length == 0)
                throw new Exception("No audio files found"); // TODO: Localize

            _tracks = new ObservableCollection<AlbumTrack>();
            foreach (string file in files)
            {
                AlbumTrack track = new AlbumTrack(file);
                _tracks.Add(track);
            }

            SetAlbumPropertiesFromTracks();
        }

        public ImageSource Picture
        {
            get { return Get<ImageSource>("Picture"); }
            set { Set("Picture", value); }
        }

        private void SetAlbumPropertiesFromTracks()
        {
            AlbumTrack first = _tracks[0];

            AlbumTitle = first.Album;
            Genre = first.Genre;
            Artist = first.Artist;
            ReleaseDate = first.ReleaseDate;
            if (first.Picture != null)
            {
                Picture = new Picture(first.Picture).ImageSource;
            }

            for (int i = 1; i < _tracks.Count; i++)
            {
                AlbumTrack track = _tracks[i];

                if (AlbumTitle != track.Album)
                    AlbumTitle = null;
                if (Genre != track.Genre)
                    Genre = null;
                if (Artist != track.Artist)
                {
                    IsVariousArtists = true;
                    Artist = null;
                }
                if (ReleaseDate != track.ReleaseDate)
                    ReleaseDate = null;
            }

            // TODO: Use Album Artist from tracks
            // TODO: Use TCMP from tracks
        }

        public ObservableCollection<AlbumTrack> Tracks
        {
            get { return _tracks; }
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

        public bool IsVariousArtists
        {
            get { return Get<bool>("IsVariousArtists"); }
            set { Set("IsVariousArtists", value); }
        }

        public void Finish()
        {
            foreach (AlbumTrack track in Tracks)
            {
                string ext = Path.GetExtension(track.OriginalFileName);

                if (!string.IsNullOrWhiteSpace(track.TrackNumber) &&
                    !string.IsNullOrWhiteSpace(track.Artist) &&
                    !string.IsNullOrWhiteSpace(track.Title))
                {
                    string newFileName = string.Format("{0:00} - {1} - {2}{3}", int.Parse(track.TrackNumber), track.Artist, track.Title, ext);
                    newFileName = Path.Combine(OldDirectoryName, newFileName);
                    if (string.Compare(track.OriginalFileName, newFileName, ignoreCase: false) != 0)
                    {
                        File.Move(track.OriginalFileName, newFileName);
                    }
                }
            }
        }
    }
}
