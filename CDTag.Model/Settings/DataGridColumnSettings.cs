using System.Runtime.Serialization;
using System.Windows.Controls;

namespace CDTag.Common.Settings
{
    /// <summary>
    /// DataGridColumnSettings
    /// </summary>
    [DataContract]
    public class DataGridColumnSettings
    {
        /// <summary>Gets or sets the width.</summary>
        /// <value>The width.</value>
        [DataMember]
        public double Width { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the column width is expressed in star notation.
        /// </summary>
        /// <value><c>true</c> if the column width is expressed in star notation; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool IsStar { get; set; }
        
        /// <summary>Gets or sets the display index.</summary>
        /// <value>The display index.</value>
        [DataMember]
        public int DisplayIndex { get; set; }
    }
}
