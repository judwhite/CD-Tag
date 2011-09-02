using System.Runtime.Serialization;

namespace CDTag.Common.Tests.Json.Model
{
    [DataContract]
    public class FillingItems
    {
        [DataMember]
        public Filling[] Filling { get; set; }
    }
}
