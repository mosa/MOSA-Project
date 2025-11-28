using System.ComponentModel;

namespace System.Runtime.InteropServices;

[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete("CurrencyWrapper and support for marshalling to the VARIANT type may be unavailable in future releases.")]
public sealed class CurrencyWrapper
{
	public decimal WrappedObject
	{
		get
		{
			throw null;
		}
	}

	public CurrencyWrapper(decimal obj)
	{
	}

	public CurrencyWrapper(object obj)
	{
	}
}
