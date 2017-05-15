pragma solidity ^0.4.0;

contract DeviceMovement {

	mapping (bytes32=>Telemetry[]) public telemetryCollection;

	struct Telemetry{
	   string lat;
	   string long;
	   int temperatureInCelcius;
	   string deviceId;
	   int64 timeSpanInTicks;
	   address sender;
    }

	function StoreTelemetry(bytes32 key, string lat, string long, int temperatureInCelcius, string deviceId, int64 timeSpanInTicks) returns (bool success)
    {
	   var telemetry = Telemetry(lat, long, temperatureInCelcius, deviceId, timeSpanInTicks, msg.sender);
	   telemetryCollection[key].push(telemetry);      
	   return true;
	} 
}
