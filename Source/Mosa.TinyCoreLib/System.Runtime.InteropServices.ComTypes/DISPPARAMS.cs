using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
public struct DISPPARAMS
{
	public int cArgs;

	public int cNamedArgs;

	public IntPtr rgdispidNamedArgs;

	public IntPtr rgvarg;
}
