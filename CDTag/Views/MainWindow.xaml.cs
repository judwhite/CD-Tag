using CDTag.ViewModel.Tag;

namespace CDTag.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowViewBase
    {
        public MainWindow(ITagViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();

            HandleEscape = false;
        }
    }
}
