using System.Runtime.Versioning;

namespace System.Threading;

[UnsupportedOSPlatform("browser")]
public sealed class RegisteredWaitHandle : MarshalByRefObject
{
	internal RegisteredWaitHandle()
	{
	}

	public bool Unregister(WaitHandle? waitObject)
	{
		throw null;
	}
}
