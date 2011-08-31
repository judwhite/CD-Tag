using System.Runtime.Serialization;

namespace CDTag.Common.Tests.Json.Model
{
    [DataContract]
    public class PhoneNumber
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Number { get; set; }
    }
}
