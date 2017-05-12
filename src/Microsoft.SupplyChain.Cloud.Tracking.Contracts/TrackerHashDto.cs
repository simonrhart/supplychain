using System;
using Microsoft.SupplyChain.Framework.Dto;
using Nethereum.Hex.HexTypes;

namespace Microsoft.SupplyChain.Cloud.Tracking.Contracts
{
    public class TrackerHashDto : BaseDto<TrackerHashDto>
    {
        public TrackerHashDto(string id, string transactionHash, string deviceId, DateTime timeStamp, HexBigInteger blockNumber, string blockHash, HexBigInteger transactionIndex, string contractAddressUsed) : base(id)
        {
            TransactionHash = transactionHash;
            DeviceId = deviceId;
            TimeStamp = timeStamp;
            BlockNumber = blockNumber;
            BlockHash = blockHash;
            TransactionIndex = transactionIndex;
            ContractAddressUsed = contractAddressUsed;
        }

        /// <summary>
        /// Gets the blockchain transaction hash of this tracking information.
        /// </summary>
        public string TransactionHash { get;}

        /// <summary>
        /// Gets the device ID to which this transactionhash belongs.
        /// </summary>
        public string DeviceId { get; }

        /// <summary>
        /// Gets the timestamp of when this transaction hash was recorded.
        /// </summary>
        public DateTime TimeStamp { get; }
        public HexBigInteger BlockNumber { get; }
        public string BlockHash { get; }
        public HexBigInteger TransactionIndex { get; }
        public string ContractAddressUsed { get; }
    }
}
