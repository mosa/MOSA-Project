using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[StructLayout(LayoutKind.Explicit)]
[EditorBrowsable(EditorBrowsableState.Never)]
public struct BINDPTR
{
	[FieldOffset(0)]
	public IntPtr lpfuncdesc;

	[FieldOffset(0)]
	public IntPtr lptcomp;

	[FieldOffset(0)]
	public IntPtr lpvardesc;
}
