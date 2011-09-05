using System.Windows.Input;
using CDTag.Common;
using CDTag.Model.Tag;
using CDTag.ViewModel.Events;

namespace CDTag.ViewModel.Tag.TagAlbum
{
    public class TagAlbumViewModel : ViewModelBase<TagAlbumViewModel>, ITagAlbumViewModel
    {
        private readonly DelegateCommand _finishCommand;
        private readonly DelegateCommand _previousCommand;
        private readonly DelegateCommand _previewNFOCommand;

        public TagAlbumViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _finishCommand = new DelegateCommand(Finish);
            _previewNFOCommand = new DelegateCommand(PreviewNFO);
            _previousCommand = new DelegateCommand(Previous);

            var getDirectoryControllerEvent = new GetDirectoryControllerEvent();
            eventAggregator.Publish(getDirectoryControllerEvent);
            string path = getDirectoryControllerEvent.DirectoryController.CurrentDirectory;
            Album = new Album(path);

            EnhancedPropertyChanged += TagAlbumViewModel_EnhancedPropertyChanged;
        }

        private void TagAlbumViewModel_EnhancedPropertyChanged(object sender, EnhancedPropertyChangedEventArgs<TagAlbumViewModel> e)
        {
            if (e.IsProperty(p => p.SelectedTrackIndex))
            {
                SelectedTrack = Album.Tracks[SelectedTrackIndex];
            }
        }

        private void Finish()
        {
            Album.Finish();

            CloseWindow();
        }

        private void PreviewNFO()
        {
        }

        private void Previous()
        {
        }

        public Album Album
        {
            get { return Get<Album>("Album"); }
            private set { Set("Album", value); }
        }

        public AlbumTrack SelectedTrack
        {
            get { return Get<AlbumTrack>("SelectedTrack"); }
            set { Set("SelectedTrack", value); }
        }

        public int SelectedTrackIndex
        {
            get { return Get<int>("SelectedTrackIndex"); }
            set { Set("SelectedTrackIndex", value); }
        }

        public ICommand FinishCommand
        {
            get { return _finishCommand; }
        }

        public ICommand PreviewNFOCommand
        {
            get { return _previewNFOCommand; }
        }

        public ICommand PreviousCommand
        {
            get { return _previousCommand; }
        }
    }
}
