using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
public struct STATSTG
{
	public FILETIME atime;

	public long cbSize;

	public Guid clsid;

	public FILETIME ctime;

	public int grfLocksSupported;

	public int grfMode;

	public int grfStateBits;

	public FILETIME mtime;

	public string pwcsName;

	public int reserved;

	public int type;
}
