using System.Globalization;

namespace Microsoft.SupplyChain.Services.Contracts
{
    public class Sensor : Base<Sensor>
    {
        public Sensor()
        {
        }

        public Sensor(string name, string gatewayId, string gpsLat, string gpsLong)
        {
            Name = name;
            GatewayId = gatewayId;          
            GpsLat = gpsLat;
            GpsLong = gpsLong;
        }
      
        public string GatewayId { get; set; }             

        public string GpsLat { get; set; }

        public string GpsLong { get; set; }
    }
}
