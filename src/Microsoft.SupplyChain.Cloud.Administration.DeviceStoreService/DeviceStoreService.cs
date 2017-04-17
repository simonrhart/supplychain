using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.SupplyChain.Cloud.Administration.Contracts;
using Microsoft.SupplyChain.Framework.Command;

namespace Microsoft.SupplyChain.Cloud.Administration.DeviceStoreService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class DeviceStoreService : StatelessService, IDeviceStoreService
    {
        private readonly IDeviceStoreRepository _deviceStoreRepository;

        public DeviceStoreService(StatelessServiceContext context, IDeviceStoreRepository deviceStoreRepository)
            : base(context)
        {
            _deviceStoreRepository = deviceStoreRepository;
        }

        /// <summary>
        /// We use .NET remoting to make this more secure. Only trusted services running within the Service Fabric cluster are
        /// able to call this service.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[] {new ServiceInstanceListener(this.CreateServiceRemotingListener)};

        }

        public async Task<DeviceTwinTagsDto> GetDeviceTwinTagsByIdAsync(string id)
        {
            return await _deviceStoreRepository.GetDeviceTwinTagsByIdAsync(id);
        }

        public async Task<Device> GetDeviceByIdAsync(string id)
        {
            return await _deviceStoreRepository.GetDeviceByIdAsync(id);
        }

        protected override async Task RunAsync(CancellationToken cancellationToken)
        {


            long iterations = 0;
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
