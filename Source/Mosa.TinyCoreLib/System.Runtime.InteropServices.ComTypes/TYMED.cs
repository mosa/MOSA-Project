using System.ComponentModel;

namespace System.Runtime.InteropServices.ComTypes;

[EditorBrowsable(EditorBrowsableState.Never)]
[Flags]
public enum TYMED
{
	TYMED_NULL = 0,
	TYMED_HGLOBAL = 1,
	TYMED_FILE = 2,
	TYMED_ISTREAM = 4,
	TYMED_ISTORAGE = 8,
	TYMED_GDI = 0x10,
	TYMED_MFPICT = 0x20,
	TYMED_ENHMF = 0x40
}
