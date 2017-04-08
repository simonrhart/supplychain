namespace Microsoft.SupplyChain.Cloud.Gateway.Contracts
{
    public abstract class Base<TGraphType>
    {
     
        public string Id { get; set; }
      
        public string Name { get; set; }
     
        public string Type => typeof (TGraphType).Name;
    }
}
