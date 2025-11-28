using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
[Flags]
public enum LIBFLAGS : short
{
	LIBFLAG_FRESTRICTED = 1,
	LIBFLAG_FCONTROL = 2,
	LIBFLAG_FHIDDEN = 4,
	LIBFLAG_FHASDISKIMAGE = 8
}
