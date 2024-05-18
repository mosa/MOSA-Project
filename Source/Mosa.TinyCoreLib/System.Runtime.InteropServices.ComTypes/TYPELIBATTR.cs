using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
public struct TYPELIBATTR
{
	public Guid guid;

	public int lcid;

	public SYSKIND syskind;

	public LIBFLAGS wLibFlags;

	public short wMajorVerNum;

	public short wMinorVerNum;
}
