using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.SupplyChain.Cloud.Tracking.Contracts;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.SupplyChain.Framework.Mvc;

namespace Microsoft.SupplyChain.Cloud.Tracking.TrackerStoreService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class TrackerStoreService : StatelessService, ITrackerStoreService
    {
        private readonly ITrackerStoreRepository _trackerStoreRepository;

        public TrackerStoreService(StatelessServiceContext context, ITrackerStoreRepository trackerStoreRepository)
            : base(context)
        {
            _trackerStoreRepository = trackerStoreRepository;
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[]
            {
                new ServiceInstanceListener(this.CreateServiceRemotingListener),
                new ServiceInstanceListener(serviceContext => new OwinCommunicationListener(StartupWithWindsor.ConfigureApp,
                    serviceContext, Framework.ServiceEventSource.Current, "ServiceEndpoint")),

            };
        }

        public async Task PublishAsync(TrackerHashDto trackerHashDto)
        {
            if (trackerHashDto == null)
                throw new ArgumentNullException(nameof(trackerHashDto));

            await _trackerStoreRepository.UpdateAsync(trackerHashDto);
        }
    }
}
