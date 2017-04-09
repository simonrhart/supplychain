using System;
using System.Collections.ObjectModel;
using System.Fabric.Description;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Security;
using Microsoft.SupplyChain.Cloud.Registration.DiscoveryService.Repositories;
using Microsoft.SupplyChain.Framework.Command;

namespace Microsoft.SupplyChain.Cloud.Registration.DiscoveryService.Commands
{
    public class GenerateIoTSecurityTokenCommand : BaseCommand<GenerateIoTSecurityTokenContext>
    {
        private readonly IDeviceStoreRepository _deviceStoreRepository;
        private readonly IDiscoveryService _discoveryService;
        private string _iotHubConnectionString;
        private KeyedCollection<string, ConfigurationProperty> _iotHubSection;

        public GenerateIoTSecurityTokenCommand(IDeviceStoreRepository deviceStoreRepository, IDiscoveryService discoveryService)
        {
            _deviceStoreRepository = deviceStoreRepository;
            _discoveryService = discoveryService;
        }

        protected override async Task DoExecuteAsync(GenerateIoTSecurityTokenContext context)
        {
            var deviceTags = await _deviceStoreRepository.GetDeviceTwinTagsById(context.Id);
            
            // check the passed mac address matches what we have in the device twin.
            if (deviceTags.MacAddress != context.MacAddress)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent($"MacAddress in device twin does not match the MacAddress of {context.MacAddress} passed"),
                    ReasonPhrase = "Critical Exception"
                });
            }

            // now generate SAS token.

            var builder = IotHubConnectionStringBuilder.Create(_iotHubConnectionString);

            int tokenExpiry = 8;
            if (context.TokenExpiryInHours != 0)
                tokenExpiry = context.TokenExpiryInHours;

            var device = await _deviceStoreRepository.GetDeviceByIdAsync(context.Id);
            SharedAccessSignatureBuilder sasBuilder;
            if (context.SignUsingPrimaryKey)
            {
                sasBuilder = new SharedAccessSignatureBuilder()
                {
                    Key = device.Authentication.SymmetricKey.PrimaryKey,
                    Target = $"{builder.HostName}/devices/{WebUtility.UrlEncode(context.Id)}",
                    TimeToLive = TimeSpan.FromHours(Convert.ToDouble(tokenExpiry))
                };
            }
            else
            {
                // then use secondary key.
                sasBuilder = new SharedAccessSignatureBuilder()
                {
                    Key = device.Authentication.SymmetricKey.SecondaryKey,
                    Target = $"{builder.HostName}/devices/{WebUtility.UrlEncode(context.Id)}",
                    TimeToLive = TimeSpan.FromHours(Convert.ToDouble(tokenExpiry))
                };
            }

            context.SasToken = $"HostName={builder.HostName};DeviceId={context.Id};SharedAccessSignature={sasBuilder.ToSignature()}";
        }

        protected override void DoInitialize(GenerateIoTSecurityTokenContext context)
        {
            var configurationPackage = _discoveryService.Context.CodePackageActivationContext.GetConfigurationPackageObject("Config");
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
            base.DoInitialize(context);
        }

        protected override ExceptionAction HandleError(GenerateIoTSecurityTokenContext context, Exception exception)
        {
            return ExceptionAction.Rethrow;
        }
    }
}
