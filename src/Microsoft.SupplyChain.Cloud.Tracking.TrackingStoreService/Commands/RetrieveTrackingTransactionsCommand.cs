using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Amqp.Serialization;
using Microsoft.Azure.Devices;
using Microsoft.SupplyChain.Cloud.Administration.Contracts;
using Microsoft.SupplyChain.Cloud.Tracking.Contracts;
using Microsoft.SupplyChain.Cloud.Tracking.TrackingStoreService.ServiceAgents;
using Microsoft.SupplyChain.Framework.Command;

namespace Microsoft.SupplyChain.Cloud.Tracking.TrackingStoreService.Commands
{
    public class RetrieveTrackingTransactionsCommand : BaseCommand<RetrieveTrackingTransactionsContext>
    {
        private readonly ITrackerStoreRepository _trackerStoreRepository;
        private readonly IDeviceStoreServiceAgent _deviceStoreServiceAgent;
        private readonly IDeviceMovementServiceAgent _deviceMovementServiceAgent;

        public RetrieveTrackingTransactionsCommand(ITrackerStoreRepository trackerStoreRepository, 
                                                   IDeviceStoreServiceAgent deviceStoreServiceAgent, 
                                                   IDeviceMovementServiceAgent deviceMovementServiceAgent)
        {
            _trackerStoreRepository = trackerStoreRepository;
            _deviceStoreServiceAgent = deviceStoreServiceAgent;
            _deviceMovementServiceAgent = deviceMovementServiceAgent;
        }

        protected override async Task DoExecuteAsync(RetrieveTrackingTransactionsContext context)
        {
            DeviceTwinTagsDto device = await _deviceStoreServiceAgent.GetDeviceTwinTagsByIdAsync(context.DeviceId);

            // get all the hashes and id's for the time period and device type passed.
            List<TrackerHashDto> hashes =
                _trackerStoreRepository.GetHashsByTime(context.DeviceId, context.From, context.To);

            foreach (var hash in hashes)
            {
               
            }

        }

        protected override ExceptionAction HandleError(RetrieveTrackingTransactionsContext context, Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
