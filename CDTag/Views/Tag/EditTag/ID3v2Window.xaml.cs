using System.Windows;
using CDTag.Model.Tag;
using CDTag.ViewModel.Tag.EditTag;
using IdSharp.Common.Events;
using Microsoft.Win32;

namespace CDTag.Views.Tag.EditTag
{
    /// <summary>
    /// Interaction logic for ID3v2Window.xaml
    /// </summary>
    public partial class ID3v2Window : WindowViewBase
    {
        private readonly OpenFileDialog _openFileDialog = new OpenFileDialog();

        public ID3v2Window(IID3v2ViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();

            CollectionNavigator.BeforeAdd += CollectionNavigator_BeforeAdd;
            CollectionNavigator.BeforeDelete += CollectionNavigator_BeforeDelete;

            _openFileDialog.Filter = "*.jpg;*.png;*.gif;*.bmp|*.jpg;*.png;*.gif;*.bmp";
        }

        private static void CollectionNavigator_BeforeDelete(object sender, CancelDataEventArgs<object> e)
        {
            // TODO: Localize
            if (MessageBox.Show("Delete picture?", "Delete picture", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void CollectionNavigator_BeforeAdd(object sender, CancelDataEventArgs<object> e)
        {
            if (_openFileDialog.ShowDialog() != true)
            {
                e.Cancel = true;
                return;
            }

            Picture picture = new Picture(_openFileDialog.FileName);
            if (picture.ImageSource != null)
                e.Data = picture;
            else
                e.Cancel = true;
        }
    }
}
