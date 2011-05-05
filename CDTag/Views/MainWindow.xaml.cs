﻿using System;
using System.Windows;
using System.Windows.Input;
using CDTag.Common;
using CDTag.Common.Settings;
using CDTag.Common.Settings.MainWindow;
using CDTag.FileBrowser.ViewModel;
using CDTag.ViewModel.Tag;
using System.Collections.Generic;

namespace CDTag.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowViewBase
    {
        private readonly ITagViewModel _viewModel;

        public MainWindow(ITagViewModel viewModel)
            : base(viewModel)
        {
            _viewModel = viewModel;

            InitializeComponent();

            HandleEscape = false;

            Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            // Splitter/InitialDirectory settings
            MainWindowSettings settings = new MainWindowSettings();
            settings.GridSplitterPosition = tagView.FileExplorer.GridSplitterPosition;
            settings.Directory = tagView.FileExplorer.DirectoryController.CurrentDirectory;

            // Column settings
            settings.ColumnSettings = new Dictionary<string, DataGridColumnSettings>();
            foreach (var item in tagView.FileExplorer.GetFileViewColumns())
            {
                string name = item.GetValue(DataGridUtil.NameProperty).ToString();
                settings.ColumnSettings.Add(name, new DataGridColumnSettings { Width = item.Width.IsAbsolute ? item.ActualWidth : item.Width, DisplayIndex = item.DisplayIndex });
            }

            SettingsFile.Save("mainWindow.json", settings);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            KeyBindingHelper.SetKeyBindings(this, tagView.TagToolbar.wrenchMenu.Items);

            InputBindings.Add(new KeyBinding(_viewModel.DirectoryViewModel.GoBackCommand, Key.Left, ModifierKeys.Alt));
            InputBindings.Add(new KeyBinding(_viewModel.DirectoryViewModel.GoForwardCommand, Key.Right, ModifierKeys.Alt));
            InputBindings.Add(new KeyBinding(_viewModel.DirectoryViewModel.GoUpCommand, Key.Up, ModifierKeys.Alt));
            InputBindings.Add(new KeyBinding(_viewModel.DirectoryViewModel.SelectAllCommand, Key.A, ModifierKeys.Control));

            MainWindowSettings settings;
            if (SettingsFile.TryLoad("mainWindow.json", out settings))
            {
                try
                {
                    // Splitter/InitialDirectory settings
                    tagView.FileExplorer.GridSplitterPosition = settings.GridSplitterPosition;

                    IDirectoryController directoryController = tagView.FileExplorer.DirectoryController; // TODO: Need a better way to get this.
                    directoryController.InitialDirectory = settings.Directory;

                    // Column settings
                    var columns = tagView.FileExplorer.GetFileViewColumns();

                    foreach (var column in columns)
                    {
                        string name = column.GetValue(DataGridUtil.NameProperty).ToString();
                        foreach (var kvp in settings.ColumnSettings)
                        {
                            if (kvp.Key == name)
                            {
                                column.Width = kvp.Value.Width;
                                column.DisplayIndex = kvp.Value.DisplayIndex;
                            }
                        }
                    }
                }
                catch
                {
                    // TODO: Log error?
                }
            }
        }
    }
}
