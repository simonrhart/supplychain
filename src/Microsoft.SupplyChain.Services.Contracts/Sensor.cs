using System;
using System.Globalization;

namespace Microsoft.SupplyChain.Services.Contracts
{
    public class Sensor : Base<Sensor>
    {
        public Sensor()
        {
        }

        public Sensor(string lat, string lg, int temperatureInCelcius, string deviceId, DateTime timeStamp)
        {   
            DeviceId = deviceId;          
            GpsLat = lat;
            GpsLong = lg;
            Timestamp = timeStamp;
            TemperatureInCelcius = temperatureInCelcius;
        }
      
        public string DeviceId { get; set; }             

        public string GpsLat { get; set; }

        public string GpsLong { get; set; }

        public DateTime Timestamp { get; set; }

        public int TemperatureInCelcius { get; set; }
    }
}
