using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using CDTag.Common;
using CDTag.FileBrowser.ViewModel;
using CDTag.ViewModel.Tag;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

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

            Closed += MainWindow_Closed;
        }

        public class MainWindowSettings
        {
            public GridLength GridSplitterPosition { get; set; }
            public string Directory { get; set; }
            public Dictionary<string, DataGridColumnSettings> ColumnSettings { get; set; }
        }

        public class DataGridColumnSettings
        {
            public DataGridLength Width { get; set; }
            public int DisplayIndex { get; set; }
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            string fileName = Path.Combine(Unity.Resolve<IApp>().LocalApplicationDirectory, "mainWindow.json");

            // Splitter/InitialDirectory settings
            MainWindowSettings settings = new MainWindowSettings();
            settings.GridSplitterPosition = tagView.FileExplorer.GridSplitterPosition;
            settings.Directory = tagView.FileExplorer.DirectoryController.CurrentDirectory;
            settings.ColumnSettings = new Dictionary<string, DataGridColumnSettings>();

            // Column settings
            foreach (var item in tagView.FileExplorer.GetFileViewColumns())
            {
                string name = item.GetValue(DataGridUtil.NameProperty).ToString();
                settings.ColumnSettings.Add(name, new DataGridColumnSettings { Width = item.Width.IsAbsolute ? item.ActualWidth : item.Width, DisplayIndex = item.DisplayIndex });
            }

            string json = JsonConvert.SerializeObject(settings, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            using (Stream fileStream = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                // Splitter/InitialDirectory settings
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            string fileName = Path.Combine(Unity.Resolve<IApp>().LocalApplicationDirectory, "mainWindow.json");
            if (File.Exists(fileName))
            {
                try
                {
                    // Splitter/InitialDirectory settings
                    string readJson = File.ReadAllText(fileName, Encoding.UTF8);
                    var settings = JsonConvert.DeserializeObject<MainWindowSettings>(readJson);
                    tagView.FileExplorer.GridSplitterPosition = settings.GridSplitterPosition;

                    IDirectoryController directoryController = tagView.FileExplorer.DirectoryController;
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
                }
            }
        }
    }
}
