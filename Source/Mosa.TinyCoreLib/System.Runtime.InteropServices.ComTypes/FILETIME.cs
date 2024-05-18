using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
public struct FILETIME
{
	public int dwHighDateTime;

	public int dwLowDateTime;
}
