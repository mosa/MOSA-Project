using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
public struct PARAMDESC
{
	public IntPtr lpVarValue;

	public PARAMFLAG wParamFlags;
}
