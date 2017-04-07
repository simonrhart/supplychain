pragma solidity ^0.4.0;

contract DeviceMovement {

	mapping (bytes32=>Telemetry[]) public telemetry;

	struct Telemetry{
	   string lat;
	   string long;
	   int temperatureInCelcius;
	   address sender;
    }

	function StoreDocument(bytes32 key, string name, string description) returns (bool success)
    {
	   var telemetry = Telemetry(name, description, msg.sender);
	   documents[key].push(doc);      
	   return true;
	} 
}
