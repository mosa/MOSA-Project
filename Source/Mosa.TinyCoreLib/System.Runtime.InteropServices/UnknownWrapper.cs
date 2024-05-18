using System.ComponentModel;

namespace System.Runtime.InteropServices;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class UnknownWrapper
{
	public object? WrappedObject
	{
		get
		{
			throw null;
		}
	}

	public UnknownWrapper(object? obj)
	{
	}
}
