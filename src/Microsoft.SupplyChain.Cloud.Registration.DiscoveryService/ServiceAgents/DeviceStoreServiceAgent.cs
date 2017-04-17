using System;
using System.Fabric;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Azure.Devices;
using Microsoft.SupplyChain.Cloud.Administration.Contracts;

namespace Microsoft.SupplyChain.Cloud.Registration.DiscoveryService.ServiceAgents
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
            try
            {
                // now invoke the service fabric service.
                return await _deviceStoreService.GetDeviceTwinTagsByIdAsync(id);
            }
            catch (FabricServiceNotFoundException notFoundex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(
                        $"Unable to communicate with the DeviceStoreService to get the Device Twin tags. Reason: {notFoundex.Message} "),
                    ReasonPhrase = notFoundex.ErrorCode.ToString()
                });
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(
                        $"Unable to communicate with the DeviceStoreService to get the Device Twin tags. Reason: {ex.Message} "),
                    ReasonPhrase = "Critical Exception"
                });
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
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Unable to communicate with the DeviceStoreService to get the Device meta data. Reason: {notFoundex.Message} "),
                    ReasonPhrase = notFoundex.ErrorCode.ToString()
                });
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent($"Unable to communicate with the DeviceStoreService reason: {ex} "),
                    ReasonPhrase = "Critical Exception"
                });
            }
        }
    }
}
