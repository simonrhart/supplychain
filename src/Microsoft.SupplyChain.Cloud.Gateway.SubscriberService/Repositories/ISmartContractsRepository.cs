using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.SupplyChain.Cloud.Gateway.Contracts;

namespace Microsoft.SupplyChain.Cloud.Gateway.SubscriberService.Repositories
{
    public interface ISmartContractsRepository
    {
        List<SmartContractDto> GetAllSmartContractsByName(SmartContractName name);

        Task UpdateAsync(SmartContractDto contract);

        SmartContractDto GetLatestVersionSmartContractByName(SmartContractName name);
    }
}
