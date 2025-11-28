using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
public struct FUNCDESC
{
	public CALLCONV callconv;

	public short cParams;

	public short cParamsOpt;

	public short cScodes;

	public ELEMDESC elemdescFunc;

	public FUNCKIND funckind;

	public INVOKEKIND invkind;

	public IntPtr lprgelemdescParam;

	public IntPtr lprgscode;

	public int memid;

	public short oVft;

	public short wFuncFlags;
}
