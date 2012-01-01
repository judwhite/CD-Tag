using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CDTag.Common;
using CDTag.View;
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
            : this(IoC.Resolve<ITagViewModel>())
        {
        }

        public TagView(ITagViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();

            _tagToolbar = IoC.Resolve<TagToolbar>();
            FileExplorer.Toolbar = _tagToolbar;
            FileExplorer.PreviewMouseDown += FileExplorer_PreviewMouseDown;

            // TODO: Move to ViewModel
            viewModel.DirectoryViewModel = FileExplorer.DirectoryController;

            MainMenu.GotFocus += delegate { IoC.Resolve<IDialogService>().CloseAddressTextBox(); };
        }

        private void FileExplorer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Menu menu = Mouse.Captured as Menu;
            if (menu == null)
                IoC.Resolve<IDialogService>().CloseAddressTextBox();
        }

        public TagToolbar TagToolbar
        {
            get { return _tagToolbar; }
        }
    }
}
