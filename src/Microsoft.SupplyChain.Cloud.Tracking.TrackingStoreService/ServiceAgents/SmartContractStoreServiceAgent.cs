using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading.Tasks;
using Microsoft.SupplyChain.Cloud.Administration.Contracts;
using Microsoft.SupplyChain.Cloud.Tracking.Contracts;

namespace Microsoft.SupplyChain.Cloud.Tracking.TrackingStoreService.ServiceAgents
{
    public class SmartContractStoreServiceAgent : ISmartContractServiceAgent
    {
        private readonly ISmartContractStoreService _smartContractStoreService;

        public SmartContractStoreServiceAgent(ISmartContractStoreService smartContractStoreService)
        {
            _smartContractStoreService = smartContractStoreService ??
                                         throw new ArgumentNullException(nameof(smartContractStoreService));
        }

        public List<SmartContractDto> GetAllSmartContractsByName(SmartContractName name)
        {
            try
            {
                return _smartContractStoreService.GetAllSmartContractsByName(name);
            }
            catch (FabricServiceNotFoundException notFoundex)
            {
                throw new Exception(
                    $"Unable to communicate with the SmartContractStoreService to get the Smart contract by name. Reason: {notFoundex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to communicate with the SmartContractStoreService reason: {ex} ");
            }
        }

        public async Task UpdateAsync(SmartContractDto contract)
        {
            try
            {
                await _smartContractStoreService.UpdateAsync(contract);
            }
            catch (FabricServiceNotFoundException notFoundex)
            {
                throw new Exception(
                    $"Unable to communicate with the SmartContractStoreService to update the Smart contract. Reason: {notFoundex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to communicate with the SmartContractStoreService reason: {ex} ");
            }
        }

        public SmartContractDto GetLatestVersionSmartContractByName(SmartContractName name)
        {
            try
            {
                return _smartContractStoreService.GetLatestVersionSmartContractByName(name);
            }
            catch (FabricServiceNotFoundException notFoundex)
            {
                throw new Exception(
                    $"Unable to communicate with the SmartContractStoreService to get the Smart contract. Reason: {notFoundex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to communicate with the SmartContractStoreService reason: {ex} ");
            }
        }
    }
}
