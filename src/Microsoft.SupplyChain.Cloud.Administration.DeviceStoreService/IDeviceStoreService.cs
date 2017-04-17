using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.SupplyChain.Cloud.Administration.Contracts;
using Microsoft.SupplyChain.Framework.ServiceFabric;

namespace Microsoft.SupplyChain.Cloud.Administration.DeviceStoreService
{
    public interface IDeviceStoreService : IService, IStatelessServiceContext
    {
        Task<DeviceTwinTagsDto> GetDeviceTwinTagsById(string id);
    }
}
