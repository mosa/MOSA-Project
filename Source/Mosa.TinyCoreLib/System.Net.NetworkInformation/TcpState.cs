namespace System.Net.NetworkInformation;

public enum TcpState
{
	Unknown,
	Closed,
	Listen,
	SynSent,
	SynReceived,
	Established,
	FinWait1,
	FinWait2,
	CloseWait,
	Closing,
	LastAck,
	TimeWait,
	DeleteTcb
}
