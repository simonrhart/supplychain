using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.SupplyChain.Cloud.Registration.Contracts;

namespace Microsoft.SupplyChain.Cloud.Registration.DiscoveryService.Repositories
{
    public interface IDeviceStoreRepository
    {
        Task<Device> GetDeviceByIdAsync(string id);

        Task<DeviceTwinTags> GetDeviceTwinTagsById(string id);
    }
}
