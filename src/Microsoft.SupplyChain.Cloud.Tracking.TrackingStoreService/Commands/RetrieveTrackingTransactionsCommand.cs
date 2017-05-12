using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.SupplyChain.Cloud.Tracking.Contracts;
using Microsoft.SupplyChain.Framework.Command;

namespace Microsoft.SupplyChain.Cloud.Tracking.TrackingStoreService.Commands
{
    public class RetrieveTrackingTransactionsCommand : BaseCommand<RetrieveTrackingTransactionsContext>
    {
        private readonly ITrackerStoreRepository _trackerStoreRepository;
        private readonly IDeviceMovementServiceAgent _deviceMovementServiceAgent;

        public RetrieveTrackingTransactionsCommand(ITrackerStoreRepository trackerStoreRepository, 
                                                   IDeviceMovementServiceAgent deviceMovementServiceAgent)
        {
            _trackerStoreRepository = trackerStoreRepository;
            _deviceMovementServiceAgent = deviceMovementServiceAgent;
        }

        protected override async Task DoExecuteAsync(RetrieveTrackingTransactionsContext context)
        {
            // get all the hashes and id's for the time period and device type passed.
            List<TrackerHashDto> hashes =
                _trackerStoreRepository.GetHashsByTime(context.DeviceId, context.From, context.To);
            
            context.TrackingCollection = await _deviceMovementServiceAgent.GetTrackingUsingIdAsync(hashes, context.DeviceId);
        }

        protected override ExceptionAction HandleError(RetrieveTrackingTransactionsContext context, Exception exception)
        {
           return ExceptionAction.Rethrow;
        }
    }
}
