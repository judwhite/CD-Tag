using CDTag.ViewModel.Options;
using CDTag.Views.Interfaces.Options;

namespace CDTag.Views.Options
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : WindowViewBase, IOptionsWindow
    {
        public OptionsWindow(IOptionsViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
