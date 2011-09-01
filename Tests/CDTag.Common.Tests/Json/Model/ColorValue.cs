using System.Runtime.Serialization;

namespace CDTag.Common.Tests.Json.Model
{
    [DataContract]
    public class ColorValue
    {
        [DataMember]
        public string Color { get; set; }

        [DataMember]
        public string Value { get; set; }
    }
}
