using System.Runtime.Serialization;

namespace CDTag.Common.Tests.Json.Model
{
    [DataContract]
    public class Address
    {
        [DataMember]
        public string StreetAddress { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public string PostalCode { get; set; }
    }
}
