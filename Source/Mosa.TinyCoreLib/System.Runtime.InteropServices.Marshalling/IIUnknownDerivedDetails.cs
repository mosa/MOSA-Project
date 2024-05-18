namespace System.Runtime.InteropServices.Marshalling;

[CLSCompliant(false)]
public interface IIUnknownDerivedDetails
{
	Guid Iid { get; }

	Type Implementation { get; }

	unsafe void** ManagedVirtualMethodTable { get; }
}
