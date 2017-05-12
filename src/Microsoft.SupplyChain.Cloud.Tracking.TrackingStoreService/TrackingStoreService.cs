using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.SupplyChain.Framework.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.SupplyChain.Cloud.Tracking.Contracts;

namespace Microsoft.SupplyChain.Cloud.Tracking.TrackingStoreService
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class TrackingStoreService : StatelessService, ITrackingStoreService
    {
        private readonly ITrackerStoreRepository _trackerStoreRepository;

        public TrackingStoreService(StatelessServiceContext context, ITrackerStoreRepository trackerStoreRepository)
            : base(context)
        {
            _trackerStoreRepository = trackerStoreRepository;
        }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[]
            {
                   new ServiceInstanceListener(serviceContext => new OwinCommunicationListener(StartupWithWindsor.ConfigureApp,
                    serviceContext, Framework.ServiceEventSource.Current, "ServiceEndpointExternal")),
                   new ServiceInstanceListener(this.CreateServiceRemotingListener, "ServiceEndpointInternal")
            };
        }

        public async Task PublishAsync(TrackerHashDto trackerHashDto)
        {
            if (trackerHashDto == null)
                throw new ArgumentNullException(nameof(trackerHashDto));

            await _trackerStoreRepository.InsertAsync(trackerHashDto);
        }
    }
}
