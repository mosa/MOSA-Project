using System.ComponentModel;
using System.Runtime.Versioning;

namespace System.Runtime.InteropServices;

[SupportedOSPlatform("windows")]
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class DispatchWrapper
{
	public object? WrappedObject
	{
		get
		{
			throw null;
		}
	}

	public DispatchWrapper(object? obj)
	{
	}
}
