namespace Microsoft.SupplyChain.Framework.Dto
{
    public abstract class BaseDto<TGraphType>
    {
        protected BaseDto(string id)
        {
            Id = id;
        }
     
        public string Id { get; }
      
        public string Name { get; set; }
     
        public string Type => typeof (TGraphType).Name;
    }
}
