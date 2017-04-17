using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.SupplyChain.Cloud.Administration.Contracts;

namespace Microsoft.SupplyChain.Cloud.Registration.DiscoveryService.DeviceStoreServiceAgent
{
    public class DeviceStoreServiceAgent : IDeviceStoreServiceAgent
    {
        private readonly IDeviceStoreService _deviceStoreService;

        public DeviceStoreServiceAgent(IDeviceStoreService deviceStoreService)
        {
            _deviceStoreService = deviceStoreService;

        }
        public async Task<DeviceTwinTagsDto> GetDeviceTwinTagsByIdAsync(string id)
        {
            return await _deviceStoreService.GetDeviceTwinTagsByIdAsync(id);
        }

        public async Task<Device> GetDeviceByIdAsync(string id)
        {
            return await _deviceStoreService.GetDeviceByIdAsync(id);
        }
    }
}
