using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
public struct ELEMDESC
{
	[StructLayout(LayoutKind.Explicit)]
	public struct DESCUNION
	{
		[FieldOffset(0)]
		public IDLDESC idldesc;

		[FieldOffset(0)]
		public PARAMDESC paramdesc;
	}

	public DESCUNION desc;

	public TYPEDESC tdesc;
}
