using System.Collections.Generic;
using System.Web.Http;
using Microsoft.SupplyChain.Framework;

namespace Microsoft.SupplyChain.Cloud.Registration.DiscoveryService.Controllers
{
    [ServiceRequestActionFilter]
    public class DiscoveryController : ApiController
    {
        private readonly ICommandAbstractFactory _factory;

       
        public DiscoveryController(ICommandAbstractFactory factory)
        {
            _factory = factory;
        }

        public string Get(int id = 0, string macAddress = null, int tokenExpirary = 0)
        {
            return id + macAddress;
        }

  
    }
}
