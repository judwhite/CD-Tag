using CDTag.ViewModels.Checksum;
using CDTag.Views.Interfaces.Checksum;

namespace CDTag.Views.Checksum
{
    /// <summary>
    /// Interaction logic for VerifyEACLogWindow.xaml
    /// </summary>
    public partial class VerifyEACLogWindow : WindowViewBase, IVerifyEACLogWindow
    {
        public VerifyEACLogWindow(IVerifyEACLogViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
