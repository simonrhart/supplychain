using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Fabric.Description;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Azure.Devices;
using Microsoft.SupplyChain.Cloud.Registration.Contracts;
using Microsoft.SupplyChain.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.SupplyChain.Cloud.Registration.DiscoveryService.Controllers
{
    [ServiceRequestActionFilter]
    public class DiscoveryController : ApiController
    {
        private readonly IDiscoveryService _discoveryService;
        private string _iotHubConnectionString;
        private readonly KeyedCollection<string, ConfigurationProperty> _iotHubSection;


        public DiscoveryController(IDiscoveryService discoveryService)
        {
            _discoveryService = discoveryService;

            var configurationPackage = _discoveryService.Context.CodePackageActivationContext.GetConfigurationPackageObject("Config");
            _iotHubSection = configurationPackage.Settings.Sections["IoTHub"].Parameters;
        }

        public async Task<string> Get(string id, string macAddress, int tokenExpirary = 0)
        {
            var iotHubConnectionString = _iotHubSection["IoTHubConnectionString"].Value;
            if (string.IsNullOrEmpty(iotHubConnectionString))
                throw new Exception("IoTHubConnectionString is not defined. Check ApplicationParameters.");

            var registryManager = RegistryManager.CreateFromConnectionString(iotHubConnectionString);
            var deviceTwin = await registryManager.GetTwinAsync(id);
           
            if (deviceTwin == null)
                throw new Exception("Unable to get DeviceTwin.");

            // get the tags for this device.
            string tags = deviceTwin.Tags.ToJson();
            DeviceTwinTags deviceTags;
            using (var sr = new StringReader(tags))
            {
                using (var jr = new JsonTextReader(sr))
                {
                    var js = new JsonSerializer();
                    deviceTags = js.Deserialize<DeviceTwinTags>(jr);
                }
            }
       
            if (deviceTags == null)
                throw new Exception($"Failed to deseralized device twin tags for device id: {id}");

            // check the passed mac address matches what we have in the device twin.
            if (deviceTags.MacAddress != macAddress)
                throw new Exception($"MacAddress in device twin does not match the MacAddress of {macAddress} passed");



            return id + macAddress;
        }

  
    }
}
