namespace System.Diagnostics.Tracing;

public enum EventOpcode
{
	Info = 0,
	Start = 1,
	Stop = 2,
	DataCollectionStart = 3,
	DataCollectionStop = 4,
	Extension = 5,
	Reply = 6,
	Resume = 7,
	Suspend = 8,
	Send = 9,
	Receive = 240
}
