using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
public struct TYPEDESC
{
	public IntPtr lpValue;

	public short vt;
}
