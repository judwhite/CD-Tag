namespace CDTag.FileBrowser.Model
{
    /// <summary>
    /// HistoryItem
    /// </summary>
    public class HistoryItem
    {
        /// <summary>Gets or sets the file view.</summary>
        /// <value>The file view.</value>
        public FileView FileView { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance represents the currently selected directory.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance represents the currently selected directory; otherwise, <c>false</c>.
        /// </value>
        public bool IsCurrent { get; set; }
    }
}
