using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using CDTag.Common;
using CDTag.Model.Tag;
using IdSharp.AudioInfo;
using IdSharp.AudioInfo.Inspection;
using IdSharp.Tagging.ID3v1;
using IdSharp.Tagging.ID3v2;
using IdSharp.Tagging.ID3v2.Frames;

namespace CDTag.ViewModel.Tag.EditTag
{
    public class ID3v2ViewModel : ViewModelBase, IID3v2ViewModel
    {
        private static readonly ObservableCollection<string> _genreCollection;
        private static readonly ObservableCollection<ID3v2TagVersion> _id3v2VersionCollection;
        private static readonly ObservableCollection<PictureType> _pictureTypeCollection;

        static ID3v2ViewModel()
        {
            _genreCollection = new ObservableCollection<string>(GenreHelper.GetSortedGenreList());
            _id3v2VersionCollection = new ObservableCollection<ID3v2TagVersion> { ID3v2TagVersion.ID3v22, ID3v2TagVersion.ID3v23, ID3v2TagVersion.ID3v24 };
            _pictureTypeCollection = new ObservableCollection<PictureType>(PictureTypeHelper.PictureTypes);
        }

        public ID3v2ViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }

        public ObservableCollection<string> GenreCollection
        {
            get { return _genreCollection; }
        }

        public string FullFileName
        {
            get { return Get<string>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public string ShortFileName
        {
            get { return Get<string>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public string EncoderPreset
        {
            get { return Get<string>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public bool CanSave
        {
            get { return Get<bool>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ObservableCollection<ID3v2TagVersion> ID3v2VersionCollection
        {
            get { return _id3v2VersionCollection; }
        }

        public ObservableCollection<PictureType> PictureTypeCollection
        {
            get { return _pictureTypeCollection; }
        }

        public ObservableCollection<Picture> PictureCollection
        {
            get { return Get<ObservableCollection<Picture>>(MethodBase.GetCurrentMethod()); }
            set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public Picture CurrentPicture
        {
            get { return Get<Picture>(MethodBase.GetCurrentMethod()); }
            set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public IID3v2Tag ID3v2
        {
            get { return Get<IID3v2Tag>(MethodBase.GetCurrentMethod()); }
            set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public IAudioFile AudioFile
        {
            get { return Get<IAudioFile>(MethodBase.GetCurrentMethod()); }
            set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        private void OnLoadFile(string fileName)
        {
            FullFileName = fileName;
            ShortFileName = Path.GetFileName(fileName);

            ID3v2 = new ID3v2Tag(fileName);
            AudioFile = IdSharp.AudioInfo.AudioFile.Create(fileName, true);
            
            if (ID3v2.PictureList == null || ID3v2.PictureList.Count == 0)
            {
                PictureCollection = new ObservableCollection<Picture>();
            }
            else
            {
                var pictureCollection = new ObservableCollection<Picture>();
                foreach (var apic in ID3v2.PictureList)
                {
                    pictureCollection.Add(new Picture(apic));
                }
                PictureCollection = pictureCollection;
            }

            // TODO: Comments
            /*Comment = null;
            if (_id3v2.CommentsList.Count > 0)
            {
                Comment = _id3v2.CommentsList[0].Value;
            }*/

            //PlayLength = audioFile.TotalSeconds;
            //Bitrate = audioFile.Bitrate;
            DescriptiveLameTagReader lameTagReader = new DescriptiveLameTagReader(fileName);
            EncoderPreset = string.Format("{0} {1}", lameTagReader.LameTagInfoEncoder, lameTagReader.UsePresetGuess == UsePresetGuess.NotNeeded ? lameTagReader.Preset : lameTagReader.PresetGuess);

            CanSave = true;
        }

        private void OnSaveFile()
        {
            List<IAttachedPicture> deleteList = new List<IAttachedPicture>(ID3v2.PictureList);

            foreach (var picture in PictureCollection)
            {
                if (picture.AttachedPicture != null)
                {
                    picture.AttachedPicture.Description = picture.Description;
                    picture.AttachedPicture.PictureType = picture.PictureType;
                    deleteList.Remove(picture.AttachedPicture);
                }
                else
                {
                    IAttachedPicture apic = ID3v2.PictureList.AddNew();
                    apic.Description = picture.Description;
                    apic.PictureType = picture.PictureType;
                    apic.PictureData = picture.PictureBytes;
                }
            }

            foreach (var deletePicture in deleteList)
            {
                ID3v2.PictureList.Remove(deletePicture);
            }

            // TODO: Multiple comments
            /*IComments comments = _id3v2.CommentsList.FirstOrDefault();
            
            if (!string.IsNullOrWhiteSpace(Comment))
            {
                if (comments == null)
                {
                    comments = _id3v2.CommentsList.AddNew();
                }
                comments.Value = Comment;
            }
            else
            {
                if (comments != null)
                    _id3v2.CommentsList.Remove(comments);
            }*/

            ID3v2.Save(FullFileName);
        }
    }
}
