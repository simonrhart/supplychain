namespace Microsoft.SupplyChain.Services.Contracts
{
    public abstract class Base<TGraphType>
    {
     
        public string Id { get; set; }
      
        public string Name { get; set; }
     
        public string Type
        {
            get { return typeof (TGraphType).Name; }
        }
    }
}
