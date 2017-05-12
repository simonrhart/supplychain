using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Cloud.Administration.Contracts
{
    public interface ISmartContractsRepository
    {
        List<SmartContractDto> GetAllSmartContractsByName(SmartContractName name);

        Task UpdateAsync(SmartContractDto contract);

        SmartContractDto GetLatestVersionSmartContractByName(SmartContractName name);
    }
}
