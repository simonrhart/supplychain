using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Services.Contracts
{    
    public class SoliditySmartContract
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Abi { get; set; }
        public int Version { get; set; }
        public string ByteCode { get; set; }

        /// <summary>
        /// Gets or sets the transaction hash of the address where the smart contract is running.
        /// </summary>
        public string Address { get; set; }

        public bool IsDeployed { get; set; }

        public SmartContractName Name { get; set; }
    }

    public enum SmartContractName
    {
        DeviceMovement
    }
}
