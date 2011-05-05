using CDTag.ViewModel.Profile.NewProfile;

namespace CDTag.Views.Profile.NewProfile
{
    /// <summary>
    /// Interaction logic for NewProfileWindow.xaml
    /// </summary>
    public partial class NewProfileWindow : WindowViewBase
    {
        public NewProfileWindow(INewProfileViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
