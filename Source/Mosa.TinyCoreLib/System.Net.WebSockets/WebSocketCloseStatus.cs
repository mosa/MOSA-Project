namespace System.Net.WebSockets;

public enum WebSocketCloseStatus
{
	NormalClosure = 1000,
	EndpointUnavailable = 1001,
	ProtocolError = 1002,
	InvalidMessageType = 1003,
	Empty = 1005,
	InvalidPayloadData = 1007,
	PolicyViolation = 1008,
	MessageTooBig = 1009,
	MandatoryExtension = 1010,
	InternalServerError = 1011
}
