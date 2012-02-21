using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using CDTag.Common;
using CDTag.ViewModels.Events;
using IdSharp.Tagging.ID3v1;

namespace CDTag.ViewModels.Tag.EditTag
{
    public class ID3v1ViewModel : ViewModelBase, IID3v1ViewModel
    {
        private static readonly ObservableCollection<string> _genreCollection;
        private static readonly ObservableCollection<ID3v1TagVersion> _id3v1VersionCollection;

        static ID3v1ViewModel()
        {
            _genreCollection = new ObservableCollection<string>(GenreHelper.GetSortedGenreList());
            _id3v1VersionCollection = new ObservableCollection<ID3v1TagVersion> { ID3v1TagVersion.ID3v10, ID3v1TagVersion.ID3v11 };
        }

        public ID3v1ViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            GetDirectoryControllerEvent args = new GetDirectoryControllerEvent();
            eventAggregator.Publish(args);

            var file = args.DirectoryController.SelectedItems.Where(p => Path.GetExtension(p.FullName).ToLower() == ".mp3").FirstOrDefault();

            if (file != null)
            {
                FileName = Path.GetFileName(file.FullName);

                ID3v1 = new ID3v1Tag(file.FullName);

                CanSave = true;
            }
            else
            {
                ID3v1 = new ID3v1Tag();
                CanSave = false;
            }
        }

        public IID3v1Tag ID3v1
        {
            get { return Get<IID3v1Tag>(MethodBase.GetCurrentMethod()); }
            set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ObservableCollection<string> GenreCollection
        {
            get { return _genreCollection; }
        }

        public string FileName
        {
            get { return Get<string>(MethodBase.GetCurrentMethod()); }
            private set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public bool CanSave
        {
            get { return Get<bool>(MethodBase.GetCurrentMethod()); }
            set { Set(MethodBase.GetCurrentMethod(), value); }
        }

        public ObservableCollection<ID3v1TagVersion> ID3v1VersionCollection
        {
            get { return _id3v1VersionCollection; }
        }

        private void OnSaveFile()
        {
            //ID3v1.Save(_fullFileName);
            //ID3v1 = new ID3v1Tag(_fullFileName);
        }
    }
}
