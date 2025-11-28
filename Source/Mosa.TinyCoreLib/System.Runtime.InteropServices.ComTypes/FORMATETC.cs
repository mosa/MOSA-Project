using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
public struct FORMATETC
{
	public short cfFormat;

	public DVASPECT dwAspect;

	public int lindex;

	public IntPtr ptd;

	public TYMED tymed;
}
