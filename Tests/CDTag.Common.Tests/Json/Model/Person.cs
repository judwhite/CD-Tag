using System.Runtime.Serialization;

namespace CDTag.Common.Tests.Json.Model
{
    [DataContract]
    public class Person
    {
        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public int Age { get; set; }

        [DataMember]
        public Address Address { get; set; }

        [DataMember]
        public PhoneNumber[] PhoneNumber { get; set; }
    }
}
