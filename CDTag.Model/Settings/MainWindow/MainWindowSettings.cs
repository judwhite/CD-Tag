using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows;

namespace CDTag.Common.Settings.MainWindow
{
    /// <summary>
    /// Settings for the application's main window.
    /// </summary>
    [DataContract]
    public class MainWindowSettings
    {
        /// <summary>Gets or sets the grid splitter position.</summary>
        /// <value>The grid splitter position.</value>
        [DataMember]
        public GridLength GridSplitterPosition { get; set; }

        /// <summary>Gets or sets the initial directory.</summary>
        /// <value>The initial directory.</value>
        [DataMember]
        public string Directory { get; set; }

        /// <summary>Gets or sets the file explorer column settings.</summary>
        /// <value>The file explorer column settings.</value>
        [DataMember]
        public Dictionary<string, DataGridColumnSettings> ColumnSettings { get; set; }
    }
}
