﻿using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CDTag.Common;
using CDTag.FileBrowser.Model;
using CDTag.FileBrowser.ViewModel;

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

                    List<UIElement> buttons = new List<UIElement>();

                    string curdir = directory;
                    while (Directory.Exists(curdir))
                    {
                        if (curdir.EndsWith(@"\") && curdir.Length > 3)
                            curdir = curdir.Substring(0, curdir.Length - 1);

                        MenuItem menuItem;
                        Menu menu;
                        GetMenuItem(curdir, directory, out menuItem, out menu);

                        if (menu != null)
                        {
                            buttons.Add(menu);
                        }

                        Button dirButton = new Button();
                        dirButton.Content = 
                            new TextBlock 
                            { 
                                Text = curdir.Length <= 3 ? DriveTypeHelper.GetDescription(new DriveInfo(curdir.Substring(0, 2))) : Path.GetFileName(curdir), 
                                Margin = new Thickness(2, 0, 2, 0) 
                            };

                        dirButton.VerticalAlignment = VerticalAlignment.Stretch;
                        dirButton.VerticalContentAlignment = VerticalAlignment.Center;
                        dirButton.Tag = curdir;
                        dirButton.Click += (s, be) => { _viewModel.CurrentDirectory = (string)((Button)s).Tag; };
                        dirButton.Style = (Style)Application.Current.Resources["DirectoryButton"];
                        buttons.Add(dirButton);

                        if (menuItem != null)
                        {
                            dirButton.MouseEnter += delegate 
                            {
                                if (!menuItem.IsSubmenuOpen)
                                {
                                    VisualStateManager.GoToState(menuItem, "MouseOver", true);
                                }
                            };
                            menuItem.MouseEnter += delegate 
                            {
                                if (!menuItem.IsSubmenuOpen)
                                {
                                    VisualStateManager.GoToState(dirButton, "MouseOver", true);
                                    VisualStateManager.GoToState(menuItem, "MouseOver", true);
                                }
                            };

                            dirButton.MouseLeave += delegate 
                            { 
                                if (!menuItem.IsMouseOver && !menuItem.IsSubmenuOpen) 
                                { 
                                    VisualStateManager.GoToState(menuItem, "Normal", true); 
                                } 
                            };
                            menuItem.MouseLeave += delegate 
                            { 
                                if (!dirButton.IsMouseOver && !menuItem.IsSubmenuOpen) 
                                { 
                                    VisualStateManager.GoToState(dirButton, "Normal", true); 
                                    VisualStateManager.GoToState(menuItem, "Normal", true); 
                                } 
                            };

                            menuItem.SubmenuOpened += delegate
                            {
                                VisualStateManager.GoToState(dirButton, "Pressed", true);
                                VisualStateManager.GoToState(menuItem, "Pressed", true);
                            };

                            menuItem.SubmenuClosed += delegate
                            {
                                if (!dirButton.IsMouseOver && !menuItem.IsMouseOver)
                                {
                                    VisualStateManager.GoToState(dirButton, "Normal", true);
                                    VisualStateManager.GoToState(menuItem, "Normal", true);
                                }
                                else
                                {
                                    VisualStateManager.GoToState(dirButton, "MouseOver", true);
                                    VisualStateManager.GoToState(menuItem, "MouseOver", true);
                                }
                            };
                        }

                        curdir = Path.GetDirectoryName(curdir);
                    }

                    MenuItem rootMenuItem;
                    Menu rootMenu;
                    GetMenuItem(curdir, directory, out rootMenuItem, out rootMenu);
                    rootMenuItem.MouseEnter += delegate
                    {
                        if (!rootMenuItem.IsSubmenuOpen)
                            VisualStateManager.GoToState(rootMenuItem, "MouseOver", true);
                    };

                    rootMenuItem.MouseLeave += delegate
                    {
                        if (!rootMenuItem.IsSubmenuOpen)
                        {
                            VisualStateManager.GoToState(rootMenuItem, "Normal", true);
                        }
                    };

                    rootMenuItem.SubmenuOpened += delegate
                    {
                        VisualStateManager.GoToState(rootMenuItem, "Pressed", true);
                    };

                    rootMenuItem.SubmenuClosed += delegate
                    {
                        if (!rootMenuItem.IsMouseOver)
                        {
                            VisualStateManager.GoToState(rootMenuItem, "Normal", true);
                        }
                        else
                        {
                            VisualStateManager.GoToState(rootMenuItem, "MouseOver", true);
                        }
                    };


                    buttons.Add(rootMenu);

                    FileView currentFileView = new FileView(directory);
                    buttons.Add(new Image { Source = currentFileView.ImageSource, Margin = new Thickness(3, 3, 3, 3), VerticalAlignment = VerticalAlignment.Center});

                    for (int i = buttons.Count - 1, j = 0; i >= 0; i--, j++)
                    {
                        ButtonGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                        buttons[i].SetValue(Grid.ColumnProperty, j);
                        ButtonGrid.Children.Add(buttons[i]);
                    }
                }
            }
        }

        private void GetMenuItem(string curdir, string directory, out MenuItem menuItem, out Menu menu)
        {
            // TODO: Put "Path" as resource

            bool isDrive;
            string[] subdirs;
            if (curdir != null)
            {
                subdirs = Directory.GetDirectories(curdir);
                isDrive = false;
            }
            else
            {
                subdirs = DriveInfo.GetDrives().Select(p => p.Name).ToArray();
                isDrive = true;
            }

            if (subdirs.Length == 0)
            {
                menuItem = null;
                menu = null;
                return;
            }

            menuItem = new MenuItem();
            menuItem.VerticalAlignment = VerticalAlignment.Stretch;
            menuItem.VerticalContentAlignment = VerticalAlignment.Center;
            menuItem.Style = (Style)Application.Current.Resources["DirectoryMenuItemStyle"];
            menuItem.Icon = new System.Windows.Shapes.Path { Data = (Geometry)Application.Current.Resources["RightArrow"], Height = 7, Width = 3.5, Fill = Brushes.Black, Margin = new Thickness(2, 0, 2, 0) };

            foreach (string dir in subdirs)
            {
                if (!isDrive)
                {
                    if (new DirectoryInfo(dir).Attributes.HasFlag(FileAttributes.Hidden))
                        continue;
                }

                bool isCurrent = (directory.StartsWith(dir));

                TextBlock header = new TextBlock 
                    { 
                        Text = isDrive ? DriveTypeHelper.GetDescription(new DriveInfo(dir)) : Path.GetFileName(dir), 
                        FontWeight = isCurrent ? FontWeights.Bold : FontWeights.Normal 
                    };
                MenuItem subDirMenuItem = new MenuItem { Header = header, Icon = new Image { Source = new FileView(dir).ImageSource }, Tag = dir };
                subDirMenuItem.Click += delegate { ((IDirectoryController)DataContext).CurrentDirectory = (string)subDirMenuItem.Tag; };
                menuItem.Items.Add(subDirMenuItem);
            }

            menu = new Menu();
            menu.VerticalAlignment = VerticalAlignment.Stretch;
            menu.VerticalContentAlignment = VerticalAlignment.Center;
            menu.Style = (Style)Application.Current.Resources["MenuStyle"];
            menu.Items.Add(menuItem);
        }
    }
}
