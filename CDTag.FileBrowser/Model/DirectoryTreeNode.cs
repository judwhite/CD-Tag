using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CDTag.Common.Win32API;

namespace CDTag.FileBrowser.Model
{
    internal class DirectoryTreeNode : TreeViewItem
    {
        private string _directory;

        public bool IsAccessDenied { get; private set; }
        public bool IsDrive { get; private set; }
        public string AccessDeniedMessage { get; private set; }

        public string Directory
        {
            get { return _directory; }
            set
            {
                _directory = value;

                Win32.SHFILEINFO info; //= new Win32.SHFILEINFO();
                Win32.SHGetFileInfo(_directory, 0, out info, (uint)Marshal.SizeOf(typeof(Win32.SHFILEINFO)), Win32.SHGFI_ICON | Win32.SHGFI_TYPENAME | Win32.SHGFI_SMALLICON);

                ImageSource img;
                using (System.Drawing.Icon icon = System.Drawing.Icon.FromHandle(info.hIcon))
                {
                    img = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, new Int32Rect(0, 0, icon.Width, icon.Height), BitmapSizeOptions.FromEmptyOptions());
                }

                string text = Path.GetFileName(_directory);

                if (string.IsNullOrEmpty(text))
                {
                    IsDrive = true;

                    DriveInfo drive = new DriveInfo(_directory);

                    text = DriveTypeHelper.GetDescription(drive);

                    if (drive.IsReady)
                    {
                        if (System.IO.Directory.GetDirectories(_directory).Length != 0)
                        {
                            Items.Add(null);
                        }
                    }
                }
                else
                {
                    string[] dirs;
                    try
                    {
                        dirs = System.IO.Directory.GetDirectories(_directory);
                        if (dirs.Length != 0)
                        {
                            Items.Add(null);
                        }
                    }
                    catch (Exception ex)
                    {
                        IsAccessDenied = true;
                        AccessDeniedMessage = ex.Message;
                    }
                }

                Header = BuildHeader(img, text);
            }
        }

        private static StackPanel BuildHeader(ImageSource img, string text)
        {
            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;

            Image icon = new Image();
            icon.VerticalAlignment = VerticalAlignment.Center;
            icon.Margin = new Thickness(0, 0, 4, 0);
            icon.Source = img;
            stack.Children.Add(icon);

            TextBlock textBlock = new TextBlock();
            textBlock.VerticalAlignment = VerticalAlignment.Center;
            textBlock.Text = text;
            textBlock.Margin = new Thickness(0, 0, 4, 0);
            stack.Children.Add(textBlock);

            stack.Margin = new Thickness(0, 1.5, 0, 1.5);

            return stack;
        }

        public static readonly RoutedEvent CollapsingEvent = EventManager.RegisterRoutedEvent("Collapsing", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DirectoryTreeNode));

        public static readonly RoutedEvent ExpandingEvent = EventManager.RegisterRoutedEvent("Expanding", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DirectoryTreeNode));

        public event RoutedEventHandler Collapsing
        {
            add { AddHandler(CollapsingEvent, value); }
            remove { RemoveHandler(CollapsingEvent, value); }
        }

        public event RoutedEventHandler Expanding
        {
            add { AddHandler(ExpandingEvent, value); }
            remove { RemoveHandler(ExpandingEvent, value); }
        }

        protected override void OnExpanded(RoutedEventArgs e)
        {
            OnExpanding(new RoutedEventArgs(ExpandingEvent, this));
            base.OnExpanded(e);
        }

        protected override void OnCollapsed(RoutedEventArgs e)
        {
            OnCollapsing(new RoutedEventArgs(CollapsingEvent, this));
            base.OnCollapsed(e);
        }

        protected virtual void OnCollapsing(RoutedEventArgs e) { RaiseEvent(e); }
        protected virtual void OnExpanding(RoutedEventArgs e) { RaiseEvent(e); }
    }
}
