namespace System.Runtime.InteropServices.Marshalling;

public enum MarshalMode
{
	Default,
	ManagedToUnmanagedIn,
	ManagedToUnmanagedRef,
	ManagedToUnmanagedOut,
	UnmanagedToManagedIn,
	UnmanagedToManagedRef,
	UnmanagedToManagedOut,
	ElementIn,
	ElementRef,
	ElementOut
}
