using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Microsoft.SupplyChain.Cloud.Administration.Contracts
{
    public interface ISmartContractStoreService : IService
    {
        List<SmartContractDto> GetAllSmartContractsByName(SmartContractName name);

        Task UpdateAsync(SmartContractDto contract);

        SmartContractDto GetLatestVersionSmartContractByName(SmartContractName name);
    }
}
