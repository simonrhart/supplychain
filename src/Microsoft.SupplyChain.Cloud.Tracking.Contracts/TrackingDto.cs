using System;
using Microsoft.SupplyChain.Framework.Dto;

namespace Microsoft.SupplyChain.Cloud.Tracking.Contracts
{
    public class TrackingDto
    {
        public TrackingDto(string transactionId, string gpsLat, string gpsLong, int temperatureInCelcius, string addressSender, string deviceId, string transactionHash, DateTime timeStamp)
        {
            TransactionId = transactionId;
            GpsLat = gpsLat;
            GpsLong = gpsLong;
            TemperatureInCelcius = temperatureInCelcius;
            AddressSender = addressSender;
            DeviceId = deviceId;
            TransactionHash = transactionHash;
            TimeStamp = timeStamp;
        }

        public string TransactionId { get; set; }
        public string GpsLat { get; set; }

        public string GpsLong { get; set; }

        public int TemperatureInCelcius { get; set; }

        /// <summary>
        /// Gets the blockchain address of the sender who created this transaction data on the chain.
        /// </summary>
        public string AddressSender { get; set; }

        public string DeviceId { get; set; }

        public string TransactionHash { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
