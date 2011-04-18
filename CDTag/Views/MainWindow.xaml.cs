using System.Windows;
using CDTag.FileBrowser;

namespace CDTag.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            FileExplorer.DirectoryController.NavigateTo(@"C:\");
        }
    }
}
