namespace System.Runtime.InteropServices.Marshalling;

[CLSCompliant(false)]
public interface IComExposedClass
{
	unsafe static abstract ComWrappers.ComInterfaceEntry* GetComInterfaceEntries(out int count);
}
