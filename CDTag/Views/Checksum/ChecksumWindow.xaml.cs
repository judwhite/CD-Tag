using CDTag.ViewModel.Checksum;

namespace CDTag.Views.Checksum
{
    /// <summary>
    /// Interaction logic for ChecksumWindow.xaml
    /// </summary>
    public partial class ChecksumWindow : WindowViewBase
    {
        public ChecksumWindow(IChecksumViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
