using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SupplyChain.Cloud.Administration.Contracts;
using Microsoft.SupplyChain.Cloud.Tracking.Contracts;
using Nethereum.Web3;

namespace Microsoft.SupplyChain.Cloud.Tracking.TrackingStoreService.ServiceAgents
{
    public class EthereumDeviceMovementServiceAgent : IDeviceMovementServiceAgent
    {
        private bool _disposed;
        private readonly Web3 _web3;
        private readonly ISmartContractStoreServiceAgent _smartContractServiceAgent;
        private readonly IDeviceStoreServiceAgent _deviceStoreServiceAgent;
        private string _contractAddress = null;
        private SmartContractDto _deviceMovementSmartContract;
        private Contract _contract;
        private Function _telemetryCollectionFunction;
     
        public EthereumDeviceMovementServiceAgent(Web3 web3,
            ISmartContractStoreServiceAgent smartContractServiceAgent,
            IDeviceStoreServiceAgent deviceStoreServiceAgent)

        {
            _smartContractServiceAgent = smartContractServiceAgent ??
                                         throw new ArgumentNullException(nameof(smartContractServiceAgent));
            _deviceStoreServiceAgent = deviceStoreServiceAgent ??
                                       throw new ArgumentNullException(nameof(deviceStoreServiceAgent));
            _web3 = web3;
        }

        public async Task<List<TrackingDto>> GetTrackingUsingIdAsync(List<TrackerHashDto> trackerHashCollection,
            string deviceId)
        {
            // get the latest smart contract version to invoke.
            _deviceMovementSmartContract =
                await _smartContractServiceAgent.GetLatestVersionSmartContractByName(SmartContractName.DeviceMovement);

            // if it's been removed since we bootstrapped the application, redeploy it.
            if (!_deviceMovementSmartContract.IsDeployed)
                throw new Exception(
                    $"Smart contract {_deviceMovementSmartContract.Name} version {_deviceMovementSmartContract.Version} has not been deployed.");

            // now load the contract using the contract address
            _contract = _web3.Eth.GetContract(_deviceMovementSmartContract.Abi,
                _deviceMovementSmartContract.Address);

            _telemetryCollectionFunction = _contract.GetFunction("telemetryCollection");
            
            var deviceTwin = await _deviceStoreServiceAgent.GetDeviceTwinTagsByIdAsync(deviceId);

            // unlock the account.
            var unlockResult = await _web3.Personal.UnlockAccount.SendRequestAsync(deviceTwin.BlockchainAccount,
                "Monday01", 1000);

            if (!unlockResult)
                throw new Exception(
                    $"Unable to unlock account {deviceTwin.BlockchainAccount} check passphrase is correct and that the account is valid");

            List<TrackingDto> trackingDtoCollection = new List<TrackingDto>();


            Task.WaitAll(trackerHashCollection.Select(t => ProcessTracker(t, trackingDtoCollection, deviceId))
                .ToArray());

            return trackingDtoCollection.OrderByDescending(x => x.TimeStamp).ToList();
        }

        private async Task ProcessTracker(TrackerHashDto t, List<TrackingDto> trackingDtoCollection, string deviceId)
        {
            var result = await _telemetryCollectionFunction.CallDeserializingToObjectAsync<TelemetrySmartContractDto>(t.TransactionId, 0);
            
            trackingDtoCollection.Add(new TrackingDto(t.TransactionId, result.GpsLat, result.GpsLong,
                result.TemperatureInCelcius, result.Sender, deviceId, t.TransactionHash, new DateTime(result.TimeSpanInTicks)));

        }

    }
}
