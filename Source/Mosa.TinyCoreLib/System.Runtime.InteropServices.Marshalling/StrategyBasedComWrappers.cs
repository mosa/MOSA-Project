using System.Collections;

namespace System.Runtime.InteropServices.Marshalling;

[CLSCompliant(false)]
public class StrategyBasedComWrappers : ComWrappers
{
	public static IIUnknownInterfaceDetailsStrategy DefaultIUnknownInterfaceDetailsStrategy
	{
		get
		{
			throw null;
		}
	}

	public static IIUnknownStrategy DefaultIUnknownStrategy
	{
		get
		{
			throw null;
		}
	}

	protected unsafe sealed override ComInterfaceEntry* ComputeVtables(object obj, CreateComInterfaceFlags flags, out int count)
	{
		throw null;
	}

	protected virtual IIUnknownCacheStrategy CreateCacheStrategy()
	{
		throw null;
	}

	protected static IIUnknownCacheStrategy CreateDefaultCacheStrategy()
	{
		throw null;
	}

	protected sealed override object CreateObject(IntPtr externalComObject, CreateObjectFlags flags)
	{
		throw null;
	}

	protected virtual IIUnknownInterfaceDetailsStrategy GetOrCreateInterfaceDetailsStrategy()
	{
		throw null;
	}

	protected virtual IIUnknownStrategy GetOrCreateIUnknownStrategy()
	{
		throw null;
	}

	protected sealed override void ReleaseObjects(IEnumerable objects)
	{
	}
}
