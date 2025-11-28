using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
public struct STGMEDIUM
{
	public object? pUnkForRelease;

	public TYMED tymed;

	public IntPtr unionmember;
}
