using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SupplyChain.Cloud.Gateway.SubscriberService.ServiceAgents;
using Microsoft.SupplyChain.Cloud.Tracking.Contracts;
using Microsoft.SupplyChain.Framework.Command;

namespace Microsoft.SupplyChain.Cloud.Tracking.TrackingStoreService.Commands
{
    public class RetrieveTrackingTransactionsCommand : BaseCommand<RetrieveTrackingTransactionsContext>
    {
        private readonly ITrackerStoreRepository _trackerStoreRepository;
        private readonly IDeviceStoreServiceAgent _deviceStoreServiceAgent;

        public RetrieveTrackingTransactionsCommand(ITrackerStoreRepository trackerStoreRepository, IDeviceStoreServiceAgent deviceStoreServiceAgent)
        {
            _trackerStoreRepository = trackerStoreRepository;
            _deviceStoreServiceAgent = deviceStoreServiceAgent;
        }

        protected override Task DoExecuteAsync(RetrieveTrackingTransactionsContext context)
        {
            
        }

        protected override ExceptionAction HandleError(RetrieveTrackingTransactionsContext context, Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
