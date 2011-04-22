using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CDTag.FileBrowser
{
    /// <summary>
    /// Interaction logic for FileList.xaml
    /// </summary>
    public partial class FileList : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileList"/> class.
        /// </summary>
        public FileList()
        {
            InitializeComponent();
        }

        internal void UpdateStatusBar(DirectoryController directoryController)
        {
            // TODO: Localization
            const string OneItem = "1 item";
            const string ManyItems = "{0:#,0} items";
            const string StatusBarKB = " ({0:#,0} KB)";
            const string StatusBarMB = " ({0:#,0.0} MB)";
            const string StatusBarGB = " ({0:#,0.0} GB)";

            if (directoryController== null)
                throw new ArgumentNullException("directoryController");

            long bytes = directoryController.DirectorySizeBytes;
            decimal kb = bytes / 1024.0m;
            decimal mb = kb / 1024.0m;
            decimal gb = mb / 1024.0m;

            int itemCount = directoryController.Count;

            string text;
            if (itemCount == 1)
                text = OneItem;
            else
                text = string.Format(ManyItems, itemCount);

            if (bytes <= 0) { /* do nothing */ }
            else if (bytes < 1024 * 1024)
                text += string.Format(StatusBarKB, kb);
            else if (bytes < 1024 * 1024 * 1024)
                text += string.Format(StatusBarMB, mb);
            else
                text += string.Format(StatusBarGB, gb);

            lblStatus.Text = text;
        }
    }
}
