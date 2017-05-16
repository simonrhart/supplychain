using System.Runtime.Serialization;

namespace Microsoft.SupplyChain.Framework.Dto
{
    [DataContract]
    public abstract class BaseDto<TGraphType>
    {
        protected BaseDto(string id)
        {
            Id = id;
        }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

       
        public string Type => typeof (TGraphType).Name;
    }
}
