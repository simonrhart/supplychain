using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.SupplyChain.Cloud.Tracking.Contracts;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace Microsoft.SupplyChain.Cloud.Tracking.TrackerStoreService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class TrackerStoreService : StatelessService, ITrackerStoreService
    {
        public TrackerStoreService(StatelessServiceContext context)
            : base(context)
        { }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[] { new ServiceInstanceListener(this.CreateServiceRemotingListener) };

        }

        public async Task Publish(TrackerHashDto trackerHashDto)
        {
            return;
        }
    }
}
