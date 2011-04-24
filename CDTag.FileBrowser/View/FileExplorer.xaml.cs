using System.Windows.Controls;
using System.Windows.Input;
using CDTag.Common;
using CDTag.FileBrowser.ViewModel;

namespace CDTag.FileBrowser.View
{
    /// <summary>
    /// Interaction logic for FileExplorer.xaml
    /// </summary>
    public partial class FileExplorer : UserControl
    {
        private readonly IDirectoryController _directoryController;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileExplorer"/> class.
        /// </summary>
        public FileExplorer()
        {
            InitializeComponent();

            _directoryController = Unity.Resolve<IDirectoryController>();
            DataContext = _directoryController;

            PreviewMouseDown += FileExplorer_PreviewMouseDown;
        }

        /// <summary>Gets or sets the toolbar.</summary>
        /// <value>The toolbar.</value>
        public object Toolbar
        {
            get { return ToolbarContentControl.Content; }
            set { ToolbarContentControl.Content = value; }
        }

        private void FileExplorer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.XButton1 == MouseButtonState.Pressed)
            {
                if (_directoryController.GoBackCommand.CanExecute(null))
                {
                    _directoryController.GoBackCommand.Execute(null);
                    e.Handled = true;
                }
            }
            else if (e.XButton2 == MouseButtonState.Pressed)
            {
                if (_directoryController.GoForwardCommand.CanExecute(null))
                {
                    _directoryController.GoForwardCommand.Execute(null);
                    e.Handled = true;
                }
            }
        }

        /// <summary>Gets the directory controller.</summary>
        public IDirectoryController DirectoryController
        {
            get { return _directoryController; }
        }
    }
}
