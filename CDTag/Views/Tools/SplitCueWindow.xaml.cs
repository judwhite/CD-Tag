using CDTag.ViewModel.Tools;

namespace CDTag.Views.Tools
{
    /// <summary>
    /// Interaction logic for SplitCueWindow.xaml
    /// </summary>
    public partial class SplitCueWindow : WindowViewBase
    {
        public SplitCueWindow(ISplitCueViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
