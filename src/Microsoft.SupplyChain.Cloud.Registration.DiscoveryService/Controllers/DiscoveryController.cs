using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Fabric.Description;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Security;
using Microsoft.SupplyChain.Cloud.Registration.Contracts;
using Microsoft.SupplyChain.Cloud.Registration.DiscoveryService.Commands;
using Microsoft.SupplyChain.Cloud.Registration.DiscoveryService.Repositories;
using Microsoft.SupplyChain.Framework;
using Microsoft.SupplyChain.Framework.Command;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.SupplyChain.Cloud.Registration.DiscoveryService.Controllers
{
    [ServiceRequestActionFilter]
    public class DiscoveryController : ApiController
    {
        private readonly ICommand<GenerateIoTSecurityTokenContext> _generateIoTTokenCommand;

        public DiscoveryController(ICommand<GenerateIoTSecurityTokenContext> generateIoTTokenCommand)
        {
            _generateIoTTokenCommand = generateIoTTokenCommand;
        }

        public async Task<string> Get(string id, string macAddress, bool signUsingPrimaryKey = true, int tokenExpiryInHours = 0)
        {
            GenerateIoTSecurityTokenContext context =
                new GenerateIoTSecurityTokenContext(id, macAddress, signUsingPrimaryKey, tokenExpiryInHours);
            await _generateIoTTokenCommand.ExecuteAsync(context);
            return context.SasToken;
        }

  
    }
}
