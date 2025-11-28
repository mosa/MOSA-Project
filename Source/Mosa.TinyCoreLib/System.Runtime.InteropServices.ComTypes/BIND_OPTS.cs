using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
public struct BIND_OPTS
{
	public int cbStruct;

	public int dwTickCountDeadline;

	public int grfFlags;

	public int grfMode;
}
