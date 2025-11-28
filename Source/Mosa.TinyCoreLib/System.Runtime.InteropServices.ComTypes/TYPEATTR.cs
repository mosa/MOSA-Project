using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
public struct TYPEATTR
{
	public short cbAlignment;

	public int cbSizeInstance;

	public short cbSizeVft;

	public short cFuncs;

	public short cImplTypes;

	public short cVars;

	public int dwReserved;

	public Guid guid;

	public IDLDESC idldescType;

	public int lcid;

	public IntPtr lpstrSchema;

	public const int MEMBER_ID_NIL = -1;

	public int memidConstructor;

	public int memidDestructor;

	public TYPEDESC tdescAlias;

	public TYPEKIND typekind;

	public short wMajorVerNum;

	public short wMinorVerNum;

	public TYPEFLAGS wTypeFlags;
}
