using System.Collections.Generic;
using System.Fabric;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.SupplyChain.Cloud.Administration.Contracts;

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
            return new[] { new ServiceInstanceListener(this.CreateServiceRemotingListener) };

        }

        public Task<DeviceTwinTagsDto> GetDeviceTwinTagsById(string id)
        {
            return _deviceStoreRepository.GetDeviceTwinTagsById(id);
        }
    }
}
