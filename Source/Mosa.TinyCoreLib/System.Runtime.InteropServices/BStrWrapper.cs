using System.ComponentModel;

namespace System.Runtime.InteropServices;

[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class BStrWrapper
{
	public string? WrappedObject
	{
		get
		{
			throw null;
		}
	}

	public BStrWrapper(object? value)
	{
	}

	public BStrWrapper(string? value)
	{
	}
}
