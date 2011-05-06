using CDTag.ViewModel.Checksum;

namespace CDTag.Views.Checksum
{
    /// <summary>
    /// Interaction logic for VerifyEACLogWindow.xaml
    /// </summary>
    public partial class VerifyEACLogWindow : WindowViewBase
    {
        public VerifyEACLogWindow(IVerifyEACLogViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
