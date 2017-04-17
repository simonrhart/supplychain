using System.Threading.Tasks;
using Microsoft.Azure.Devices;

namespace Microsoft.SupplyChain.Cloud.Administration.Contracts
{
    public interface IDeviceStoreRepository
    {
        Task<Device> GetDeviceByIdAsync(string id);

        Task<DeviceTwinTagsDto> GetDeviceTwinTagsById(string id);
    }
}
