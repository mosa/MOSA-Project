namespace System.Runtime.InteropServices.Marshalling;

[CLSCompliant(false)]
public interface IUnmanagedVirtualMethodTableProvider
{
	VirtualMethodTableInfo GetVirtualMethodTableInfoForKey(Type type);
}
