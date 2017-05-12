using System;
using Microsoft.SupplyChain.Framework.Dto;

namespace Microsoft.SupplyChain.Cloud.Tracking.Contracts
{
    public class TrackingDto : BaseDto<TrackingDto>
    {
        public TrackingDto(string id, string gpsLat, string gpsLong, int temperatureInCelcius, string addressSender, string deviceId, string transactionHash, DateTime timeStamp) : base(id)
        {
            GpsLat = gpsLat;
            GpsLong = gpsLong;
            TemperatureInCelcius = temperatureInCelcius;
            AddressSender = addressSender;
            DeviceId = deviceId;
            TransactionHash = transactionHash;
            TimeStamp = timeStamp;
        }

        public string GpsLat { get; }

        public string GpsLong { get; }

        public int TemperatureInCelcius { get; }

        /// <summary>
        /// Gets the blockchain address of the sender who created this transaction data on the chain.
        /// </summary>
        public string AddressSender { get; }

        public string DeviceId { get; }

        public string TransactionHash { get; }

        public DateTime TimeStamp { get; }
    }
}
