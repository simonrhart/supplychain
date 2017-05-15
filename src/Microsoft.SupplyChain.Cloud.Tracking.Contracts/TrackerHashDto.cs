using System;
using System.Runtime.Serialization;
using Microsoft.SupplyChain.Framework.Dto;
using Nethereum.Hex.HexTypes;

namespace Microsoft.SupplyChain.Cloud.Tracking.Contracts
{
    [DataContract]
    public class TrackerHashDto : BaseDto<TrackerHashDto>
    {
        public TrackerHashDto(string id, string transactionHash, string deviceId, DateTime timeStamp, string blockNumber, string blockHash, string transactionIndex, string contractAddressUsed) : base(id)
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
        [DataMember]
        public string TransactionHash { get; set; }

        /// <summary>
        /// Gets the device ID to which this transactionhash belongs.
        /// </summary>
        [DataMember]
        public string DeviceId { get; set; }

        /// <summary>
        /// Gets the timestamp of when this transaction hash was recorded.
        /// </summary>
        [DataMember]
        public DateTime TimeStamp { get; set; }
        [DataMember]
        public string BlockNumber { get; set; }
        [DataMember]
        public string BlockHash { get; set; }
        [DataMember]
        public string TransactionIndex { get; set; }
        [DataMember]
        public string ContractAddressUsed { get; set; }
    }
}
