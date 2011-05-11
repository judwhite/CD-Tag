using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CDTag.Common;
using CDTag.ViewModel.Tag;

namespace CDTag.Views.Tag
{
    /// <summary>
    /// Interaction logic for TagView.xaml
    /// </summary>
    public partial class TagView : ViewBase
    {
        private readonly TagToolbar _tagToolbar;

        public TagView()
            : this(Unity.Resolve<ITagViewModel>())
        {
        }

        public TagView(ITagViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();

            _tagToolbar = Unity.Resolve<TagToolbar>();
            FileExplorer.Toolbar = _tagToolbar;
            FileExplorer.PreviewMouseDown += FileExplorer_PreviewMouseDown;

            // TODO: Move to ViewModel
            viewModel.DirectoryViewModel = FileExplorer.DirectoryController;
        }

        private void FileExplorer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Menu menu = Mouse.Captured as Menu;
            if (menu == null)
                Unity.App.CloseAddressTextBox();
        }

        public TagToolbar TagToolbar
        {
            get { return _tagToolbar; }
        }
    }
}
