using System;
using Microsoft.SupplyChain.Framework.Dto;

namespace Microsoft.SupplyChain.Cloud.Tracking.Contracts
{
    public class TrackerHashDto : BaseDto<TrackerHashDto>
    {
        public TrackerHashDto(string id, string transactionHash, string deviceId, DateTime timeStamp) : base(id)
        {
            TransactionHash = transactionHash;
            DeviceId = deviceId;
            TimeStamp = timeStamp;
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
    }
}
