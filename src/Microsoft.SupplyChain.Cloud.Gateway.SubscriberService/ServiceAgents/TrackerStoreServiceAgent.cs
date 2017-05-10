using System;
using System.Fabric;
using System.Threading.Tasks;
using Microsoft.SupplyChain.Cloud.Tracking.Contracts;

namespace Microsoft.SupplyChain.Cloud.Gateway.SubscriberService.ServiceAgents
{
    public class TrackerStoreServiceAgent : ITrackerStoreServiceAgent
    {
        private readonly ITrackingStoreService _trackerStoreService;

        public TrackerStoreServiceAgent(ITrackingStoreService trackerStoreService)
        {
            _trackerStoreService = trackerStoreService;
        }

        public Task Publish(TrackerHashDto trackerHashDto)
        {
            try
            {
                // now invoke the service fabric service.
                return _trackerStoreService.PublishAsync(trackerHashDto);
            }
            catch (FabricServiceNotFoundException notFoundex)
            {
                throw new Exception(
                    $"Unable to communicate with the TrackerStoreService to publish tracker hashes. Reason: {notFoundex.Message} ");

            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Unable to communicate with the TrackerStoreService to publish tracker hashes. Reason: {ex.Message}");
            }

        }
    }
}
