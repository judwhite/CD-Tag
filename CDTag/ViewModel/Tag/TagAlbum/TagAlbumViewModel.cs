using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CDTag.Common;
using CDTag.Events;
using CDTag.Model.Tag;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;

namespace CDTag.ViewModel.Tag.TagAlbum
{
    public class TagAlbumViewModel : ViewModelBase, ITagAlbumViewModel
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

            var getDirectoryControllerEventArgs = new GetDirectoryControllerEventArgs();
            eventAggregator.GetEvent<GetDirectoryControllerEvent>().Publish(getDirectoryControllerEventArgs);
            string path = getDirectoryControllerEventArgs.DirectoryController.CurrentDirectory;
            Album = new Album(path);
        }

        private void Finish()
        {
            Album.Finish();
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
