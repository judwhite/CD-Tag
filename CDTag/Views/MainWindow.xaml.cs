using System.Windows;
using CDTag.ViewModel.Tag;

namespace CDTag.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(ITagViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
