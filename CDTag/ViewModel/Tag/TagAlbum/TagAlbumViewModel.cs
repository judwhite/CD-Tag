using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CDTag.Common;
using Microsoft.Practices.Prism.Events;

namespace CDTag.ViewModel.Tag.TagAlbum
{
    public class TagAlbumViewModel : ViewModelBase, ITagAlbumViewModel
    {
        public TagAlbumViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }
    }
}
