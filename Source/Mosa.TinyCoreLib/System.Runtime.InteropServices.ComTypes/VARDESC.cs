using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
public struct VARDESC
{
	[StructLayout(LayoutKind.Explicit)]
	public struct DESCUNION
	{
		[FieldOffset(0)]
		public IntPtr lpvarValue;

		[FieldOffset(0)]
		public int oInst;
	}

	public DESCUNION desc;

	public ELEMDESC elemdescVar;

	public string lpstrSchema;

	public int memid;

	public VARKIND varkind;

	public short wVarFlags;
}
