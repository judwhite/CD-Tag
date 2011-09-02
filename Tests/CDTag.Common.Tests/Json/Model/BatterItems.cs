using System.Runtime.Serialization;

namespace CDTag.Common.Tests.Json.Model
{
    [DataContract]
    public class BatterItems
    {
        [DataMember]
        public Ingredient[] Batter { get; set; }
    }
}
