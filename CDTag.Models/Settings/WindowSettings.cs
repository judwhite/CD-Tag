using System.Runtime.Serialization;
using System.Windows;

namespace CDTag.Common.Settings
{
    /// <summary>
    /// WindowSettings
    /// </summary>
    [DataContract]
    public class WindowSettings
    {
        /// <summary>Gets or sets the height.</summary>
        /// <value>The height.</value>
        [DataMember]
        public double? Height { get; set; }

        /// <summary>Gets or sets the width.</summary>
        /// <value>The width.</value>
        [DataMember]
        public double? Width { get; set; }

        /// <summary>Gets or sets the top.</summary>
        /// <value>The top.</value>
        [DataMember]
        public double? Top { get; set; }

        /// <summary>Gets or sets the left.</summary>
        /// <value>The left.</value>
        [DataMember]
        public double? Left { get; set; }

        /// <summary>Gets or sets the state of the window.</summary>
        /// <value>The state of the window.</value>
        [DataMember]
        public WindowState? WindowState { get; set; }
    }
}
