﻿using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Microsoft.SupplyChain.Cloud.Administration.Contracts
{
    public interface IDeviceStoreService : IService
    {
        Task<DeviceTwinTagsDto> GetDeviceTwinTagsByIdAsync(string id);

        Task<Device> GetDeviceByIdAsync(string id);
    }
}
