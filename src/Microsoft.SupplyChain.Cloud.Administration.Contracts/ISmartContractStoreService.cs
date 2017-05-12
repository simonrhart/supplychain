using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Microsoft.SupplyChain.Cloud.Administration.Contracts
{
    public interface ISmartContractStoreService : IService
    {
        Task<List<SmartContractDto>> GetAllSmartContractsByNameAsync(SmartContractName name);

        Task UpdateAsync(SmartContractDto contract);

        Task<SmartContractDto> GetLatestVersionSmartContractByNameAsync(SmartContractName name);
    }
}
