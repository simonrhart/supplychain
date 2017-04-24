using Newtonsoft.Json;

namespace Microsoft.SupplyChain.Cloud.Gateway.Contracts
{
    public class SmartContractDto
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Abi { get; set; }
        public double Version { get; set; }
        public string ByteCode { get; set; }

        /// <summary>
        /// Gets or sets the transaction hash of the address where the smart contract is running.
        /// </summary>
        public string Address { get; set; }

        public bool IsDeployed { get; set; }

        public SmartContractName Name { get; set; }
    }

   
}
