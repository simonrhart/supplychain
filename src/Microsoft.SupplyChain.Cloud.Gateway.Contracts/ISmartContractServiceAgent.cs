using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.SupplyChain.Cloud.Administration.Contracts;

namespace Microsoft.SupplyChain.Cloud.Gateway.Contracts
{
    public interface ISmartContractServiceAgent
    {
        List<SmartContractDto> GetAllSmartContractsByName(SmartContractName name);

        Task UpdateAsync(SmartContractDto contract);

        SmartContractDto GetLatestVersionSmartContractByName(SmartContractName name);
    }
}
