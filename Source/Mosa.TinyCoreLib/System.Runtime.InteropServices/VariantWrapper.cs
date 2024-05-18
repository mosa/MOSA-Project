using System.ComponentModel;

namespace System.Runtime.InteropServices;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class VariantWrapper
{
	public object? WrappedObject
	{
		get
		{
			throw null;
		}
	}

	public VariantWrapper(object? obj)
	{
	}
}
