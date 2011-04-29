using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using CDTag.Common;
using CDTag.Common.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CDTag.Views
{
    public class WindowViewBase : Window
    {
        private bool _settingsLoaded;

        protected WindowViewBase(IViewModelBase viewModel)
        {
            DataContext = viewModel;

            PreviewKeyDown += WindowViewBase_PreviewKeyDown;
            Closed += WindowViewBase_Closed;
            HandleEscape = true;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            if (!_settingsLoaded)
                LoadWindowSettings();
        }

        public bool HandleEscape { get; set; }

        private void LoadWindowSettings()
        {
            _settingsLoaded = true;

            string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"CD-Tag");
            string fileName = Path.Combine(directory, "windows.json");
            Dictionary<string, WindowSettings> windows;
            WindowSettings windowSettings = null;
            if (File.Exists(fileName))
            {
                try
                {
                    string readJson = File.ReadAllText(fileName, Encoding.UTF8);
                    windows = JsonConvert.DeserializeObject<Dictionary<string, WindowSettings>>(readJson);

                    if (windows.ContainsKey(Name))
                        windowSettings = windows[Name];
                }
                catch
                {
                }
            }

            if (windowSettings != null)
            {
                Height = windowSettings.Height ?? Height;
                Width = windowSettings.Width ?? Width;
                Top = windowSettings.Top ?? Top;
                Left = windowSettings.Left ?? Left;
                WindowState = (windowSettings.WindowState == null || windowSettings.WindowState == WindowState.Minimized) ? WindowState : windowSettings.WindowState.Value;
                WindowStartupLocation = WindowStartupLocation.Manual;
            }
        }

        private void WindowViewBase_Closed(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name))
                return;

            string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"CD-Tag");
            string fileName = Path.Combine(directory, "windows.json");
            Dictionary<string, WindowSettings> windows;
            if (File.Exists(fileName))
            {
                try
                {
                    string readJson = File.ReadAllText(fileName, Encoding.UTF8);
                    windows = JsonConvert.DeserializeObject<Dictionary<string, WindowSettings>>(readJson);
                }
                catch
                {
                    windows = new Dictionary<string, WindowSettings>();
                }
            }
            else
            {
                windows = new Dictionary<string, WindowSettings>();
            }

            WindowSettings windowSettings;
            if (windows.TryGetValue(Name, out windowSettings))
            {
                // Preserve old values if WindowState != Normal
                if (WindowState == WindowState.Normal)
                {
                    windowSettings.Height = Height;
                    windowSettings.Width = Width;
                    windowSettings.Top = Top;
                    windowSettings.Left = Left;
                }
                windowSettings.WindowState = WindowState;
            }
            else
            {
                windowSettings = new WindowSettings();
                windowSettings.Height = (WindowState == WindowState.Normal ? Height : (double?)null);
                windowSettings.Width = (WindowState == WindowState.Normal ? Width : (double?)null);
                windowSettings.Top = (WindowState == WindowState.Normal ? Top : (double?)null);
                windowSettings.Left = (WindowState == WindowState.Normal ? Left : (double?)null);
                windowSettings.WindowState = WindowState;

                windows.Add(Name, windowSettings);
            }

            string json = JsonConvert.SerializeObject(windows, Formatting.Indented, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            using (Stream fileStream = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }

        private void WindowViewBase_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        public WindowViewBase()
        {
            // Note: only here to support XAML
            throw new NotSupportedException();
        }
    }
}
