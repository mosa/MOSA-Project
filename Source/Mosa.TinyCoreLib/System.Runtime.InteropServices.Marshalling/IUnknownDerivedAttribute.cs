namespace System.Runtime.InteropServices.Marshalling;

[AttributeUsage(AttributeTargets.Interface, Inherited = false)]
[CLSCompliant(false)]
public class IUnknownDerivedAttribute<T, TImpl> : Attribute, IIUnknownDerivedDetails where T : IIUnknownInterfaceType
{
	public Guid Iid
	{
		get
		{
			throw null;
		}
	}

	public Type Implementation
	{
		get
		{
			throw null;
		}
	}

	public unsafe void** ManagedVirtualMethodTable
	{
		get
		{
			throw null;
		}
	}
}
