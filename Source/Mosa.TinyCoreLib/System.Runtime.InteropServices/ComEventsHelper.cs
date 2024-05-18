using System.ComponentModel;
using System.Runtime.Versioning;

namespace System.Runtime.InteropServices;

[SupportedOSPlatform("windows")]
[EditorBrowsable(EditorBrowsableState.Never)]
public static class ComEventsHelper
{
	public static void Combine(object rcw, Guid iid, int dispid, Delegate d)
	{
	}

	public static Delegate? Remove(object rcw, Guid iid, int dispid, Delegate d)
	{
		throw null;
	}
}
