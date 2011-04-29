using CDTag.ViewModel.About;

namespace CDTag.Views.About
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : WindowViewBase
    {
        public AboutWindow(IAboutViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();

            viewModel.ErrorContainer = errorContainer;
        }

        public AboutWindow()
        {
            // Note: only here to support XAML
        }
    }
}
