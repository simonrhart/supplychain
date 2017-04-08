using Microsoft.Azure.Devices;
using Microsoft.SupplyChain.Cloud.Registration.Contracts;

namespace Microsoft.SupplyChain.Cloud.Registration.DiscoveryService.Repositories
{
    public class DeviceMetadataRepository : IDeviceMetadataRepository
    {
        public DeviceMetadataRepository()
        {
            
        }

        public DeviceTwinTags GetTagsById(string id)
        {
            //dynamic registryManager = RegistryManager.CreateFromConnectionString(connString);
            return null;
        }
    }
}
