namespace System.Runtime.InteropServices.Marshalling;

[CLSCompliant(false)]
public interface IIUnknownInterfaceType
{
	static abstract Guid Iid { get; }

	unsafe static abstract void** ManagedVirtualMethodTable { get; }
}
