using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.SupplyChain.Cloud.Administration.Contracts;

namespace Microsoft.SupplyChain.Cloud.Tracking.TrackingStoreService.ServiceAgents
{
    public interface IDeviceStoreServiceAgent
    {
        Task<DeviceTwinTagsDto> GetDeviceTwinTagsByIdAsync(string id);

        Task<Device> GetDeviceByIdAsync(string id);
    }
}
