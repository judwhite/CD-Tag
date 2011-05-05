using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CDTag.ViewModel.Tag.TagAlbum;

namespace CDTag.Views.Tag.TagAlbum
{
    /// <summary>
    /// Interaction logic for TagAlbumWindow.xaml
    /// </summary>
    public partial class TagAlbumWindow : WindowViewBase
    {
        public TagAlbumWindow(ITagAlbumViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
