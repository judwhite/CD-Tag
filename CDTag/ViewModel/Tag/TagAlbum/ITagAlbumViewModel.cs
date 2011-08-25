using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CDTag.Common;

namespace CDTag.ViewModel.Tag.TagAlbum
{
    public interface ITagAlbumViewModel : IViewModelBase
    {
        ICommand FinishCommand { get; }
        ICommand PreviewNFOCommand { get; }
        ICommand PreviousCommand { get; }
    }
}
