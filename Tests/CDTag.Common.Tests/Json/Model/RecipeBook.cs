using System.Runtime.Serialization;

namespace CDTag.Common.Tests.Json.Model
{
    [DataContract]
    public class RecipeBook
    {
        [DataMember]
        public RecipeItems Items { get; set; }
    }
}
