using System.Runtime.Versioning;

namespace System.Runtime.InteropServices;

public sealed class PosixSignalRegistration : IDisposable
{
	internal PosixSignalRegistration()
	{
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	[UnsupportedOSPlatform("ios")]
	[UnsupportedOSPlatform("tvos")]
	public static PosixSignalRegistration Create(PosixSignal signal, Action<PosixSignalContext> handler)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	~PosixSignalRegistration()
	{
	}
}
