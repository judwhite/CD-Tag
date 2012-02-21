using CDTag.View.Interfaces.Checksum;
using CDTag.ViewModels.Checksum;

namespace CDTag.Views.Checksum
{
    /// <summary>
    /// Interaction logic for ChecksumWindow.xaml
    /// </summary>
    public partial class ChecksumWindow : WindowViewBase, IChecksumWindow
    {
        public ChecksumWindow(IChecksumViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
