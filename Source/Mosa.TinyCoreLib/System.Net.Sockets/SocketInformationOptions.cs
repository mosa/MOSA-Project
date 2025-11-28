using System.ComponentModel;

namespace System.Net.Sockets;

[Flags]
public enum SocketInformationOptions
{
	NonBlocking = 1,
	Connected = 2,
	Listening = 4,
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("SocketInformationOptions.UseOnlyOverlappedIO has been deprecated and is not supported.")]
	UseOnlyOverlappedIO = 8
}
