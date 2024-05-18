using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
public struct CONNECTDATA
{
	public int dwCookie;

	public object pUnk;
}
