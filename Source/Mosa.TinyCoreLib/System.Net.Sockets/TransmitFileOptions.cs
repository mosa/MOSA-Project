using System.Runtime.Versioning;

namespace System.Net.Sockets;

[Flags]
public enum TransmitFileOptions
{
	UseDefaultWorkerThread = 0,
	Disconnect = 1,
	ReuseSocket = 2,
	[SupportedOSPlatform("windows")]
	WriteBehind = 4,
	[SupportedOSPlatform("windows")]
	UseSystemThread = 0x10,
	[SupportedOSPlatform("windows")]
	UseKernelApc = 0x20
}
