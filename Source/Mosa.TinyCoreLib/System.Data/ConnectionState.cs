namespace System.Data;

[Flags]
public enum ConnectionState
{
	Closed = 0,
	Open = 1,
	Connecting = 2,
	Executing = 4,
	Fetching = 8,
	Broken = 0x10
}
