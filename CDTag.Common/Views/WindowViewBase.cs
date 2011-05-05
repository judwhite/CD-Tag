using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using CDTag.Common;
using CDTag.Common.Settings;

namespace CDTag.Views
{
    /// <summary>
    /// WindowViewBase. Handles generic window settings.
    /// </summary>
    public class WindowViewBase : Window
    {
        private bool _settingsLoaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowViewBase"/> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        protected WindowViewBase(IViewModelBase viewModel)
        {
            DataContext = viewModel;

            PreviewKeyDown += WindowViewBase_PreviewKeyDown;
            Closed += WindowViewBase_Closed;
            HandleEscape = true;
            ShowInTaskbar = false;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.FrameworkElement.Initialized"/> event. This method is invoked whenever <see cref="P:System.Windows.FrameworkElement.IsInitialized"/> is set to true internally.
        /// </summary>
        /// <param name="e">The <see cref="T:System.Windows.RoutedEventArgs"/> that contains the event data.</param>
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            if (!_settingsLoaded)
                LoadWindowSettings();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the escape key should be used to close the window.
        /// </summary>
        /// <value><c>true</c> if the escape key should be used to close the window; otherwise, <c>false</c>.</value>
        public bool HandleEscape { get; set; }

        private void LoadWindowSettings()
        {
            _settingsLoaded = true;

            const string fileName = "windows.json";
            Dictionary<string, WindowSettings> windows;
            if (SettingsFile.TryLoad(fileName, out windows))
            {
                WindowSettings windowSettings;
                if (windows.TryGetValue(Name, out windowSettings))
                {
                    Height = windowSettings.Height ?? Height;
                    Width = windowSettings.Width ?? Width;
                    if (WindowStartupLocation != WindowStartupLocation.CenterOwner)
                    {
                        Top = windowSettings.Top ?? Top;
                        Left = windowSettings.Left ?? Left;
                    }
                    WindowState = (windowSettings.WindowState == null || windowSettings.WindowState == WindowState.Minimized) ? WindowState : windowSettings.WindowState.Value;
                }
            }
        }

        private void WindowViewBase_Closed(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name))
                return;

            const string fileName = "windows.json";
            Dictionary<string, WindowSettings> windows;
            if (!SettingsFile.TryLoad(fileName, out windows))
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

            SettingsFile.Save(fileName, windows);
        }

        private void WindowViewBase_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (HandleEscape)
            {
                if (e.Key == Key.Escape)
                {
                    Close();
                }
            }
        }

        /// <summary>
        /// Only here to support XAML. Use <see cref="WindowViewBase(IViewModelBase)" /> instead.
        /// </summary>
        public WindowViewBase()
        {
            // Note: only here to support XAML
            throw new NotSupportedException();
        }
    }
}
