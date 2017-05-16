using System;
using Microsoft.SupplyChain.Framework.Dto;

namespace Microsoft.SupplyChain.Cloud.Gateway.Contracts
{
    public class SensorDto 
    {
        public SensorDto(string transactionId, string lat, string lg, int temperatureInCelcius, string deviceId, DateTime timeStamp)
        {
            TransactionId = transactionId;
            DeviceId = deviceId;          
            GpsLat = lat;
            GpsLong = lg;
            Timestamp = timeStamp;
            TemperatureInCelcius = temperatureInCelcius;
        }

        public string TransactionId { get; set; }
        public string DeviceId { get; set; }             

        public string GpsLat { get; set; }

        public string GpsLong { get; set; }

        public DateTime Timestamp { get; set; }

        public int TemperatureInCelcius { get; set; }
    }
}
