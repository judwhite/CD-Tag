using System;
using CDTag.ViewModels.Profile.NewProfile;
using CDTag.Views.Interfaces.Profile.NewProfile;

namespace CDTag.Views.Profile.NewProfile
{
    /// <summary>
    /// Interaction logic for NewProfileWindow.xaml
    /// </summary>
    public partial class NewProfileWindow : WindowViewBase, INewProfileWindow
    {
        public NewProfileWindow(INewProfileViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }

        private void PageOneStoryboard_Completed(object sender, EventArgs e)
        {
            ProfileNameTextBox.Focus();
        }

        private void PageTwoStoryboard_Completed(object sender, EventArgs e)
        {
            DirectoryFormatsDataGrid.Focus();
        }
    }
}
