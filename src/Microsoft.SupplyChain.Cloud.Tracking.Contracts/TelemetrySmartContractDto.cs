using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Microsoft.SupplyChain.Cloud.Tracking.Contracts
{
    [FunctionOutput]
    public class TelemetrySmartContractDto
    {
        [Parameter("string", "lat", 1)]
        public string GpsLat { get; set; }

        [Parameter("string", "long", 2)]
        public string GpsLong { get; set; }

        [Parameter("int", "temperatureInCelcius", 3)]
        public int TemperatureInCelcius { get; set; }

        [Parameter("string", "deviceId", 4)]
        public string DeviceId { get; set; }

        [Parameter("int64", "timeSpanInTicks", 5)]
        public long TimeSpanInTicks { get; set; }
        
        [Parameter("address", "sender", 5)]
        public string Sender { get; set; }
    }
}
