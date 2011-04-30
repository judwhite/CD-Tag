using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CDTag.Common;
using CDTag.FileBrowser.ViewModel;
using Path = System.IO.Path;

namespace CDTag.FileBrowser.View
{
    /// <summary>
    /// Interaction logic for DirectoryButtons.xaml
    /// </summary>
    public partial class DirectoryButtons : UserControl
    {
        private IDirectoryController _viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryButtons"/> class.
        /// </summary>
        public DirectoryButtons()
        {
            InitializeComponent();

            DataContextChanged += DirectoryButtons_DataContextChanged;
        }

        private void DirectoryButtons_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _viewModel = (IDirectoryController)DataContext;
            _viewModel.EnhancedPropertyChanged += _viewModel_EnhancedPropertyChanged;
        }

        private void _viewModel_EnhancedPropertyChanged(object sender, EnhancedPropertyChangedEventArgs<IDirectoryController> e)
        {
            if (e.IsProperty(p => p.CurrentDirectory))
            {
                string directory = _viewModel.CurrentDirectory;
                if (Directory.Exists(directory))
                {
                    ButtonGrid.Children.Clear();
                    ButtonGrid.ColumnDefinitions.Clear();

                    List<Button> buttons = new List<Button>();

                    string curdir = directory;
                    bool isFirst = true;
                    while (Directory.Exists(curdir))
                    {
                        if (curdir.EndsWith(@"\") && curdir.Length > 3)
                            curdir = curdir.Substring(0, curdir.Length - 1);

                        bool addDropButton = true;
                        if (isFirst)
                        {
                            isFirst = false;

                            if (Directory.GetDirectories(curdir).Length == 0)
                                addDropButton = false;
                        }

                        if (addDropButton)
                        {
                            Button dropButton = new Button();
                            dropButton.Content = ">";
                            buttons.Add(dropButton);
                        }

                        Button dirButton = new Button();
                        dirButton.Content = curdir.Length == 3 ? curdir : Path.GetFileName(curdir);
                        dirButton.Tag = curdir;
                        dirButton.Click += (s, be) => { _viewModel.CurrentDirectory = (string)((Button)s).Tag; };
                        buttons.Add(dirButton);

                        curdir = Path.GetDirectoryName(curdir);
                    }

                    for (int i = buttons.Count - 1, j = 0; i >= 0; i--, j++)
                    {
                        ButtonGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                        buttons[i].SetValue(Grid.ColumnProperty, j);
                        ButtonGrid.Children.Add(buttons[i]);
                    }

                }
            }
        }
    }
}
