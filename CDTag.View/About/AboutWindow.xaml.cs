using CDTag.View.Interfaces.About;
using CDTag.ViewModels.About;

namespace CDTag.Views.About
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : WindowViewBase, IAboutWindow
    {
        public AboutWindow(IAboutViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();

            // TODO: Better way to let the ViewModel know about the error container?
            viewModel.ErrorContainer = errorContainer;
        }
    }
}
