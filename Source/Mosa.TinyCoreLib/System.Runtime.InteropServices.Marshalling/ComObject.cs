namespace System.Runtime.InteropServices.Marshalling;

public sealed class ComObject : IDynamicInterfaceCastable, IUnmanagedVirtualMethodTableProvider
{
	internal ComObject()
	{
	}

	public void FinalRelease()
	{
	}

	~ComObject()
	{
	}

	RuntimeTypeHandle IDynamicInterfaceCastable.GetInterfaceImplementation(RuntimeTypeHandle interfaceType)
	{
		throw null;
	}

	bool IDynamicInterfaceCastable.IsInterfaceImplemented(RuntimeTypeHandle interfaceType, bool throwIfNotImplemented)
	{
		throw null;
	}

	VirtualMethodTableInfo IUnmanagedVirtualMethodTableProvider.GetVirtualMethodTableInfoForKey(Type type)
	{
		throw null;
	}
}
