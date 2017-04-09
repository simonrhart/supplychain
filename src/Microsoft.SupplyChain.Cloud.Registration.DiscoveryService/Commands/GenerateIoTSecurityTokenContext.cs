using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SupplyChain.Framework;
using Microsoft.SupplyChain.Framework.Command;

namespace Microsoft.SupplyChain.Cloud.Registration.DiscoveryService.Commands
{
    public class GenerateIoTSecurityTokenContext : BaseContext
    {
       public GenerateIoTSecurityTokenContext(string id, string macAddress, bool signUsingPrimaryKey, int tokenExpiryInHours)
        {
            Id = id;
            MacAddress = macAddress;
            SignUsingPrimaryKey = signUsingPrimaryKey;
            TokenExpiryInHours = tokenExpiryInHours;
        }

        public string Id { get; }
        public string MacAddress { get; }
        public bool SignUsingPrimaryKey { get; }
        public int TokenExpiryInHours { get; }

        public string SasToken { get; set; }
    }
}
