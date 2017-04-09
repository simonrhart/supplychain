using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Fabric.Description;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Security;
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

        public async Task<string> Get(string id, string macAddress, bool signUsingPrimaryKey = true, int tokenExpiryInHours = 0)
        {
            var iotHubConnectionString = _iotHubSection["IoTHubConnectionString"].Value;
            if (string.IsNullOrEmpty(iotHubConnectionString))
                throw new Exception("IoTHubConnectionString is not defined. Check ApplicationParameters.");

            var registryManager = RegistryManager.CreateFromConnectionString(iotHubConnectionString);

            var device = await registryManager.GetDeviceAsync(id);
            if (device == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent($"Unable to get device Id: {id} from the identity store."),
                    ReasonPhrase = "Critical Exception"
                });

                
            }
            var deviceTwin = await registryManager.GetTwinAsync(id);

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
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"Failed to deseralized device twin tags for device id: {id}"),
                    ReasonPhrase = "Critical Exception"
                });
            }

            // check the passed mac address matches what we have in the device twin.
            if (deviceTags.MacAddress != macAddress)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent($"MacAddress in device twin does not match the MacAddress of {macAddress} passed"),
                    ReasonPhrase = "Critical Exception"
                });
            }

            // now generate SAS token.

            var builder = IotHubConnectionStringBuilder.Create(iotHubConnectionString);

            int tokenExpiry = 8;
            if (tokenExpiryInHours != 0)
                tokenExpiry = tokenExpiryInHours;

            SharedAccessSignatureBuilder sasBuilder;
            if (signUsingPrimaryKey)
            {
                sasBuilder = new SharedAccessSignatureBuilder()
                {
                    Key = device.Authentication.SymmetricKey.PrimaryKey,
                    Target = $"{builder.HostName}/devices/{WebUtility.UrlEncode(id)}",
                    TimeToLive = TimeSpan.FromHours(Convert.ToDouble(tokenExpiry))
                };
            }
            else
            {
                // then use secondary key.
                sasBuilder = new SharedAccessSignatureBuilder()
                {
                    Key = device.Authentication.SymmetricKey.SecondaryKey,
                    Target = $"{builder.HostName}/devices/{WebUtility.UrlEncode(id)}",
                    TimeToLive = TimeSpan.FromHours(Convert.ToDouble(tokenExpiry))
                };
            }

            return $"HostName={builder.HostName};DeviceId={id};SharedAccessSignature={sasBuilder.ToSignature()}";
        }

  
    }
}
