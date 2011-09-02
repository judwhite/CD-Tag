using System.Runtime.Serialization;

namespace CDTag.Common.Tests.Json.Model
{
    [DataContract]
    public class Recipe
    {
        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public decimal PPU { get; set; }

        [DataMember]
        public BatterItems Batters { get; set; }

        [DataMember]
        public Ingredient[] Topping { get; set; }

        [DataMember]
        public FillingItems Fillings { get; set; }

        [DataMember]
        public Image Image { get; set; }

        [DataMember]
        public Image Thumbnail { get; set; }
    }
}
