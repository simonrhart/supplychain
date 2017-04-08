using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SupplyChain.Cloud.Registration.Contracts;

namespace Microsoft.SupplyChain.Cloud.Registration.DiscoveryService.Repositories
{
    public interface IDeviceMetadataRepository
    {
        DeviceTwinTags GetTagsById(string id);
    }
}
