using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.SupplyChain.Cloud.Tracking.Contracts;
using Microsoft.SupplyChain.Framework.Mvc;

namespace Microsoft.SupplyChain.Cloud.Tracking.TrackingStoreService.Controllers
{
    [ServiceRequestActionFilter]
    public class TrackerReadonlyStoreController : ApiController
    {
        //// private readonly ICommand<GenerateIoTSecurityTokenContext> _generateIoTTokenCommand;

        // public TrackerReadonlyStoreController(ICommand<GenerateIoTSecurityTokenContext> generateIoTTokenCommand)
        // {
        //     _generateIoTTokenCommand = generateIoTTokenCommand;
        // }

        // public async Task<string> Get(string id, string macAddress, bool signUsingPrimaryKey = true, int tokenExpiryInHours = 0)
        // {
        //     GenerateIoTSecurityTokenContext context =
        //         new GenerateIoTSecurityTokenContext(id, macAddress, signUsingPrimaryKey, tokenExpiryInHours);
        //     try
        //     {
        //         await _generateIoTTokenCommand.ExecuteAsync(context);
        //         return context.SasToken;
        //     }
        //     catch (HttpResponseException)
        //     {
        //         throw;
        //     }
        //     catch (Exception ex)
        //     {
        //         //uncaught exception in command.
        //         throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
        //         {
        //             Content = new StringContent(
        //                 $"Unknown exception while generating security token. Reason: {ex.Message} "),
        //             ReasonPhrase = "Fatal Service Error"
        //         });
        //     }
        //}

        public List<TrackerHashDto> Get(DateTime from, DateTime to, string deviceId)
        {
            try
            {
                List<TrackerHashDto> coll = new List<TrackerHashDto>();
                coll.Add(new TrackerHashDto("1234", "1234", "1", DateTime.Now));
                return coll;

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
                        $"Unknown exception while reading from the blockchain. Reason: {ex.Message} "),
                    ReasonPhrase = "Fatal Service Error"
                });
            }
        }
    }
}
