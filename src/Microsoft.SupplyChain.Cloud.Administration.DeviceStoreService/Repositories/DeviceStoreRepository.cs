using System.Collections.ObjectModel;
using System.Fabric.Description;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
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
        private readonly IDeviceStoreService _deviceStoreService;
        private readonly string _iotHubConnectionString;
        private readonly KeyedCollection<string, ConfigurationProperty> _iotHubSection;
        private readonly RegistryManager _registryManager;

        public DeviceStoreRepository(IDeviceStoreService deviceStoreService)
        {
            _deviceStoreService = deviceStoreService;
            var configurationPackage = _deviceStoreService.Context.CodePackageActivationContext.GetConfigurationPackageObject("Config");
            _iotHubSection = configurationPackage.Settings.Sections["IoTHub"].Parameters;
            _iotHubConnectionString = _iotHubSection["IoTHubConnectionString"].Value;

            if (string.IsNullOrEmpty(_iotHubConnectionString))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("IoTHubConnectionString is not defined. Check ApplicationParameters in the Service Fabric config package."),
                    ReasonPhrase = "Critical Exception"
                });
            }

            _registryManager = RegistryManager.CreateFromConnectionString(_iotHubConnectionString);
        }

        public async Task<DeviceTwinTagsDto> GetDeviceTwinTagsById(string id)
        {
            var deviceTwin = await _registryManager.GetTwinAsync(id);

            if (deviceTwin == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Unable to get device twin for device Id: {id} from the identity store."),
                    ReasonPhrase = "Critical Exception"
                });
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
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Failed to deseralized device twin tags for device id: {id}"),
                    ReasonPhrase = "Critical Exception"
                });
            }
            return deviceTags;
        }

        public async Task<Device> GetDeviceByIdAsync(string id)
        {
            var device = await _registryManager.GetDeviceAsync(id);
            if (device == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent($"Unable to get device Id: {id} from the identity store."),
                    ReasonPhrase = "Critical Exception"
                });
            }

            return device;
        }
    }
}
