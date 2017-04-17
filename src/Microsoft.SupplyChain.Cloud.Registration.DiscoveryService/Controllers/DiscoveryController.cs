using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.SupplyChain.Cloud.Registration.DiscoveryService.Commands;
using Microsoft.SupplyChain.Framework.Command;

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
