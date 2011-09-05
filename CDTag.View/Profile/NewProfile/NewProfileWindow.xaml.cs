using CDTag.View.Interfaces.Profile.NewProfile;
using CDTag.ViewModel.Profile.NewProfile;

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
    }
}
