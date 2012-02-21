using CDTag.View.Interfaces.Checksum;
using CDTag.ViewModels.Checksum;

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
