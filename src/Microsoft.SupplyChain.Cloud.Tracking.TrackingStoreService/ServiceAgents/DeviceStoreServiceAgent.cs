using System;
using System.Fabric;
using System.Threading.Tasks;
using Microsoft.SupplyChain.Cloud.Administration.Contracts;
using Microsoft.SupplyChain.Cloud.Gateway.SubscriberService.ServiceAgents;

namespace Microsoft.SupplyChain.Cloud.Tracking.TrackingStoreService.ServiceAgents
{
    public class DeviceStoreServiceAgent : IDeviceStoreServiceAgent
    {
        private readonly IDeviceStoreService _deviceStoreService;

        public DeviceStoreServiceAgent(IDeviceStoreService deviceStoreService)
        {
            _deviceStoreService = deviceStoreService ?? throw new ArgumentNullException(nameof(deviceStoreService));
        }

        public async Task<DeviceTwinTagsDto> GetDeviceTwinTagsByIdAsync(string id)
        {
            try
            {
                // now invoke the service fabric service.
                return await _deviceStoreService.GetDeviceTwinTagsByIdAsync(id);
            }
            catch (FabricServiceNotFoundException notFoundex)
            {
                throw new Exception(
                    $"Unable to communicate with the DeviceStoreService to get the Device Twin tags. Reason: {notFoundex.Message} ");

            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Unable to communicate with the DeviceStoreService to get the Device Twin tags. Reason: {ex.Message}");
            }
        }

        public async Task<Device> GetDeviceByIdAsync(string id)
        {
            try
            {
                return await _deviceStoreService.GetDeviceByIdAsync(id);
            }
            catch (FabricServiceNotFoundException notFoundex)
            {
                throw new Exception(
                    $"Unable to communicate with the DeviceStoreService to get the Device meta data. Reason: {notFoundex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to communicate with the DeviceStoreService reason: {ex} ");
            }
        }
    }
}
