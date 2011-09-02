using System.Runtime.Serialization;

namespace CDTag.Common.Tests.Json.Model
{
    [DataContract]
    public class Ingredient
    {
        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string Type { get; set; }
    }
}
