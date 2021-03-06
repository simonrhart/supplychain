﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.SupplyChain.Cloud.Administration.Contracts;
using Microsoft.SupplyChain.Cloud.Tracking.Contracts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;

namespace Microsoft.SupplyChain.Cloud.Tracking.TrackingStoreService.ServiceAgents
{
    public class EthereumDeviceMovementServiceAgent : IDeviceMovementServiceAgent
    {
        private bool _disposed;
        private readonly Web3 _web3;
        private readonly string _blockchainAdminAccount;
        private readonly string _blockchainAdminPassphrase;
        private readonly ISmartContractStoreServiceAgent _smartContractServiceAgent;
        private string _contractAddress = null;
        private SmartContractDto _deviceMovementSmartContract;
        private Contract _contract;
        private Function _telemetryCollectionFunction;
     
        public EthereumDeviceMovementServiceAgent(Web3 web3, 
                                                  string blockchainAdminAccount, 
                                                  string blockchainAdminPassphrase,
                                                  ISmartContractStoreServiceAgent smartContractServiceAgent)
        {
            _smartContractServiceAgent = smartContractServiceAgent ??
                                         throw new ArgumentNullException(nameof(smartContractServiceAgent));
            _web3 = web3;
            _blockchainAdminAccount = blockchainAdminAccount;
            _blockchainAdminPassphrase = blockchainAdminPassphrase;
        }

        public async Task<List<TrackingDto>> GetTrackingUsingHashesAsync(List<TrackerHashDto> trackerHashCollection)
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
       
            // unlock the account.
            var unlockResult = await _web3.Personal.UnlockAccount.SendRequestAsync(_blockchainAdminAccount,
                _blockchainAdminPassphrase, 1000);

            if (!unlockResult)
                throw new Exception(
                    $"Unable to unlock account {_blockchainAdminAccount} check passphrase is correct and that the account is valid");

            List<TrackingDto> trackingDtoCollection = new List<TrackingDto>();

            Task.WaitAll(trackerHashCollection.Select(t => ProcessTracker(t, trackingDtoCollection))
                .ToArray());

            return trackingDtoCollection.OrderByDescending(x => x.TimeStamp).ToList();
        }

        private async Task ProcessTracker(TrackerHashDto t, List<TrackingDto> trackingDtoCollection)
        {
            var result = await _telemetryCollectionFunction.CallDeserializingToObjectAsync<TelemetrySmartContractDto>(t.TransactionId, 0);
            
            trackingDtoCollection.Add(new TrackingDto(t.TransactionId, result.GpsLat, result.GpsLong,
                result.TemperatureInCelcius, result.Sender, result.DeviceId, t.TransactionHash, new DateTime(result.TimeSpanInTicks)));

        }

    }
}
