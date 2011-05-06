using CDTag.ViewModel.Options;

namespace CDTag.Views.Options
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : WindowViewBase
    {
        public OptionsWindow(IOptionsViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}
