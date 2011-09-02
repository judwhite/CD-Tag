using System.Runtime.Serialization;

namespace CDTag.Common.Tests.Json.Model
{
    [DataContract]
    public class RecipeItems
    {
        [DataMember]
        public Recipe[] Item { get; set; }
    }
}
