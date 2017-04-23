using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.SupplyChain.Cloud.Gateway.Contracts;

namespace Microsoft.SupplyChain.Cloud.Gateway.SubscriberService.Repositories
{
    public interface ISmartContractsRepository
    {
        List<SoliditySmartContract> GetAllSmartContractsByName(SmartContractName name);

        Task UpdateAsync(SoliditySmartContract contract);

        SoliditySmartContract GetLatestSmartContractByName(SmartContractName name);
    }
}
