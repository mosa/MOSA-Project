namespace System.Net.Sockets;

[Flags]
public enum SocketFlags
{
	None = 0,
	OutOfBand = 1,
	Peek = 2,
	DontRoute = 4,
	Truncated = 0x100,
	ControlDataTruncated = 0x200,
	Broadcast = 0x400,
	Multicast = 0x800,
	Partial = 0x8000
}
