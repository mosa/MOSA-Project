using System.ComponentModel;

namespace System.Runtime.InteropServices.Marshalling;

[Flags]
public enum ComInterfaceOptions
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	None = 0,
	ManagedObjectWrapper = 1,
	ComObjectWrapper = 2
}
