using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.SupplyChain.Cloud.Tracking.Contracts;
using Microsoft.SupplyChain.Cloud.Tracking.TrackingStoreService.Commands;
using Microsoft.SupplyChain.Framework.Command;
using Microsoft.SupplyChain.Framework.Mvc;

namespace Microsoft.SupplyChain.Cloud.Tracking.TrackingStoreService.Controllers
{
    [ServiceRequestActionFilter]
    public class TrackerReadonlyStoreController : ApiController
    {
        private readonly ICommand<RetrieveTrackingTransactionsContext> _retrieveTrackingTransactionsCommand;

        public TrackerReadonlyStoreController(ICommand<RetrieveTrackingTransactionsContext> retrieveTrackingTransactionsCommand)
        {
            _retrieveTrackingTransactionsCommand = retrieveTrackingTransactionsCommand;
        }

        public async Task<List<TrackingDto>> Get(DateTime from, DateTime to, string deviceId)
        {
            RetrieveTrackingTransactionsContext context = new RetrieveTrackingTransactionsContext(from, to, deviceId);
            try
            {
                await _retrieveTrackingTransactionsCommand.ExecuteAsync(context);
                return context.TrackingCollection;

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
                        $"Unknown exception while searching for blockchain tracking records. Reason: {ex.Message} "),
                    ReasonPhrase = "Fatal Service Error"
                });
            }
        }
    
    }
}
