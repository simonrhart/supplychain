using System;
using System.Net;
using System.Net.Http;
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
            try
            {
                await _generateIoTTokenCommand.ExecuteAsync(context);
                return context.SasToken;
            }
            catch (HttpResponseException)
            {
                throw;
            }
            catch (Exception ex)
            {
                //uncaught exception in command.
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(
                        $"Unknown exception while generating security token. Reason: {ex.Message} "),
                    ReasonPhrase = "Fatal Service Error"
                });
            }
        }
    }
}
