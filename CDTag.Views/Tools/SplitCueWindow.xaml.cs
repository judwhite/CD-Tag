using CDTag.ViewModels.Tools;
using CDTag.Views.Interfaces.Tools;

namespace CDTag.Views.Tools
{
    /// <summary>
    /// Interaction logic for SplitCueWindow.xaml
    /// </summary>
    public partial class SplitCueWindow : WindowViewBase, ISplitCueWindow
    {
        public SplitCueWindow(ISplitCueViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
