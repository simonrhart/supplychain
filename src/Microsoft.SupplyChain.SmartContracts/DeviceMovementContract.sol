pragma solidity ^0.4.0;

contract DeviceMovement {

	mapping (bytes32=>Telemetry[]) public telemetryCollection;

	struct Telemetry{
	   string lat;
	   string long;
	   int temperatureInCelcius;
	   string deviceId;
	   address sender;
    }

	function StoreMovement(bytes32 key, string lat, string long, int temperatureInCelcius, string deviceId) returns (bool success)
    {
	   var telemetry = Telemetry(lat, long, temperatureInCelcius, deviceId, msg.sender);
	   telemetryCollection[key].push(telemetry);      
	   return true;
	} 
}
