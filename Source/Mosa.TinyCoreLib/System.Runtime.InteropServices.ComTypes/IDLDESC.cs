using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
public struct IDLDESC
{
	public IntPtr dwReserved;

	public IDLFLAG wIDLFlags;
}
