namespace System.Runtime.InteropServices.Marshalling;

[CLSCompliant(false)]
public interface IComExposedDetails
{
	unsafe ComWrappers.ComInterfaceEntry* GetComInterfaceEntries(out int count);
}
