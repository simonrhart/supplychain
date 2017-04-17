using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.SupplyChain.Cloud.Administration.Contracts;
using Newtonsoft.Json;

namespace Microsoft.SupplyChain.Cloud.Administration.DeviceStoreService.Repositories
{
    /// <summary>
    /// Wraps the identity store in IoT Hub for testability and maintainability reasons.
    /// </summary>
    public class DeviceStoreRepository : IDeviceStoreRepository
    {
        private readonly RegistryManager _registryManager;

        public DeviceStoreRepository(string iotHubConnectionString)
        {
            if (string.IsNullOrEmpty(iotHubConnectionString))
                throw new Exception(
                    "IoTHubConnectionString is not defined. Check ApplicationParameters in the Service Fabric config package.");
    
            _registryManager = RegistryManager.CreateFromConnectionString(iotHubConnectionString);
        }

        public async Task<DeviceTwinTagsDto> GetDeviceTwinTagsByIdAsync(string id)
        {
            var deviceTwin = await _registryManager.GetTwinAsync(id);

            if (deviceTwin == null)
            {
                throw new Exception($"Unable to get device twin for device Id: {id} from the identity store.");
            }

            // get the tags for this device.
            string tags = deviceTwin.Tags.ToJson();
            DeviceTwinTagsDto deviceTags;
            using (var sr = new StringReader(tags))
            {
                using (var jr = new JsonTextReader(sr))
                {
                    var js = new JsonSerializer();
                    deviceTags = js.Deserialize<DeviceTwinTagsDto>(jr);
                }
            }

            if (deviceTags == null)
            {
                throw new Exception($"Failed to deseralized device twin tags for device id: {id}");
            }
            return deviceTags;
        }

        public async Task<Device> GetDeviceByIdAsync(string id)
        {
            var device = await _registryManager.GetDeviceAsync(id);
            if (device == null)
            {
                throw new Exception($"Unable to get device Id: {id} from the identity store.");
            }

            return device;
        }
    }
}
