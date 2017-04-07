pragma solidity ^0.4.0;

contract DeviceMovement {

	mapping (bytes32=>Telemetry[]) public telemetryCollection;

	struct Telemetry{
	   string lat;
	   string long;
	   int temperatureInCelcius;
	   address sender;
    }

	function StoreMovement(bytes32 key, string lat, string long, int temperatureInCelcius) returns (bool success)
    {
	   var telemetry = Telemetry(lat, long, temperatureInCelcius, msg.sender);
	   telemetryCollection[key].push(telemetry);      
	   return true;
	} 
}
